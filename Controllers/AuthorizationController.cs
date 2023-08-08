using LibraryV2.Dto.PostDto;
using LibraryV2.Mapper;
using LibraryV2.Models;
using LibraryV2.Repository.Interfaces;
using LibraryV2.Survices;
using LibraryV2.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace LibraryV2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorizationController : ControllerBase
{
    private readonly IReaderRepository _readerRepository;
    private readonly IModelMapperService _postModelMapper;
    private readonly IConfiguration _configuration;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ReaderMapper _readerMapper = new();

    public AuthorizationController(IReaderRepository readerRepository,
                                   IModelMapperService postModelMapper,
                                   IConfiguration configuration,
                                   IRefreshTokenRepository refreshTokenRepository)
    {
        _readerRepository = readerRepository;
        _postModelMapper = postModelMapper;
        _configuration = configuration;
        _refreshTokenRepository = refreshTokenRepository;
    }

    //### POST ###

    [HttpPost("register")]
    public async Task<ActionResult<string>> RegisterReader([FromBody] ReaderPostDto readerDto)
    {
        if (readerDto is null)
        {
            ModelState.AddModelError("Entity", "Should provide item for register");
            return BadRequest(ModelState);
        }

        if (string.IsNullOrWhiteSpace(readerDto.Login))
        {
            ModelState.AddModelError("Entity", "Should provide login");
            return BadRequest(ModelState);
        }

        if (string.IsNullOrWhiteSpace(readerDto.Password))
        {
            ModelState.AddModelError("Entity", "Should provide password");
            return BadRequest(ModelState);
        }

        var login = await _readerRepository.GetReader(readerDto.Login);

        if (login is not null)
        {
            ModelState.AddModelError("Entity", "User with this login already exist");
            return BadRequest(ModelState);
        }

        var reader = _postModelMapper.ReaderPostDtoToReader(readerDto, ModelState);

        reader.PasswordHash = AuthUtility.CreatePasswordHash(readerDto.Password, _configuration.GetValue<string>("PasswordSalt")!);
        reader.Role = Roles.User;

        if (!await _readerRepository.CreateReader(reader))
        {
            return StatusCode(500);
        }

        var dto = _readerMapper.ReaderToReaderDto(reader);

        return Ok(dto);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginReader([FromBody] ReaderPostDto readerDto)
    {
        if (readerDto == null)
        {
            ModelState.AddModelError("Entity", "Should provide item for register");
            return BadRequest(ModelState);
        }

        if (string.IsNullOrWhiteSpace(readerDto.Login))
        {
            ModelState.AddModelError("Entity", "Should provide login");
            return BadRequest(ModelState);
        }

        if (string.IsNullOrWhiteSpace(readerDto.Password))
        {
            ModelState.AddModelError("Entity", "Should provide password");
            return BadRequest(ModelState);
        }
        var reader = await _readerRepository.GetReader(readerDto.Login);

        if (reader is null || !AuthUtility.VerifyPasswordHash(readerDto.Password, reader.PasswordHash, _configuration.GetValue<string>("PasswordSalt")!))
        {
            ModelState.AddModelError("Auth", "Wrong login or password");
            return BadRequest(ModelState);
        }

        var token = AuthUtility.CreateToken(reader, _configuration.GetValue<string>("JWTKey")!);

        var refreshToken = await _refreshTokenRepository.GetRefreshToken(reader);

        if (refreshToken is not null)
        {
            refreshToken = AuthUtility.UpdateRefreshToken(reader, refreshToken);

            if (!await _refreshTokenRepository.UpdateRefreshToken(refreshToken))
            {
                return StatusCode(500, ModelState);
            }
        }
        else
        {
            refreshToken = AuthUtility.GetRefreshToken(reader);

            if (!await _refreshTokenRepository.CreateRefreshToken(refreshToken))
            {
                return StatusCode(500, ModelState);
            }
        }

        AuthUtility.SetCookieWithRefreshToken(refreshToken, Response);

        reader.RefreshToken = refreshToken.Token;
        reader.TokenCreated = refreshToken.Created;
        reader.TokenExpires = refreshToken.Expires;

        if (!await _readerRepository.UpdateReader(reader))
        {
            return StatusCode(500, ModelState);
        }

        Response.Cookies.Append("readerId", reader.Id.ToString());

        var user = _readerMapper.ReaderToReaderDto(reader);

        return Ok(new { user, token });
    }

    //### GET ###

    [HttpGet("refresh_token")]
    public async Task<IActionResult> RefreshToken()
    {
        if (!Ulid.TryParse(Request.Cookies["readerId"], out Ulid id))
        {
            ModelState.AddModelError("Entity", "Wrong reader id");
            return BadRequest(ModelState);
        }

        var reader = await _readerRepository.GetReader(id);

        if (reader == null)
        {
            ModelState.AddModelError("Entity", "Not found");
            return NotFound(ModelState);
        }

        var refreshToken = Request.Cookies["refreshToken"] ?? string.Empty;

        if (reader.RefreshToken is null)
        {
            ModelState.AddModelError("Auth", "Refresh token is empty");
            return BadRequest(ModelState);
        }

        if (!reader.RefreshToken.Equals(refreshToken))
        {
            ModelState.AddModelError("Auth", "Invalid refresh token");
            return Unauthorized(ModelState);
        }

        if (reader.TokenExpires < DateTime.UtcNow)
        {
            ModelState.AddModelError("Auth", "Token expired");
            return Unauthorized(ModelState);
        }

        var oldRefreshToken = await _refreshTokenRepository.GetRefreshToken(reader);

        if (oldRefreshToken is null)
        {
            ModelState.AddModelError("Auth", "Invalid refresh token");
            return Unauthorized(ModelState);
        }

        var token = AuthUtility.CreateToken(reader, _configuration.GetValue<string>("JWTKey")!);

        var newRefreshToken = AuthUtility.UpdateRefreshToken(reader, oldRefreshToken);

        if (!await _refreshTokenRepository.UpdateRefreshToken(newRefreshToken))
        {
            return StatusCode(500, ModelState);
        }

        AuthUtility.SetCookieWithRefreshToken(newRefreshToken, Response);

        reader.RefreshToken = newRefreshToken.Token;
        reader.TokenCreated = newRefreshToken.Created;
        reader.TokenExpires = newRefreshToken.Expires;

        if (!await _readerRepository.UpdateReader(reader))
        {
            return StatusCode(500, ModelState);
        }

        var user = _readerMapper.ReaderToReaderDto(reader);

        return Ok(new { user, token });
    }

    //### DELETE ###

    [HttpDelete("{readerId}")]
    public async Task<IActionResult> RemoveReaderTokens([FromRoute] string readerId)
    {
        if (!Ulid.TryParse(readerId, out Ulid ulid))
        {
            ModelState.AddModelError("ID", "Invalid id");
            return BadRequest(ModelState);
        }

        var reader = await _readerRepository.GetReader(ulid);

        if (reader is null)
        {
            return NotFound();
        }

        var refreshToken = await _refreshTokenRepository.GetRefreshToken(reader);

        if (refreshToken is null)
        {
            return NotFound();
        }

        if (!await _refreshTokenRepository.DeleteRefreshToken(refreshToken))
        {
            return StatusCode(500);
        }

        return Ok();
    }

    [HttpDelete("remove_all")]
    public async Task<IActionResult> RemoveAllTokens()
    {
        if (!await _refreshTokenRepository.DeleteAllRefreshTokens())
        {
            return StatusCode(500);
        }

        return Ok();
    }
}
