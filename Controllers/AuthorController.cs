using LibraryV2.Dto;
using LibraryV2.Dto.PatchDto;
using LibraryV2.Dto.PostDto;
using LibraryV2.Mapper;
using LibraryV2.Models;
using LibraryV2.Repository.Implementations;
using LibraryV2.Repository.Interfaces;
using LibraryV2.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace LibraryV2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorController : ControllerBase
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IPostModelMapper _postModelMapper;
    private readonly AuthorMapper _authorMapper = new();

    public AuthorController(IAuthorRepository authorRepository,
                            IBookRepository bookRepository,
                            IPostModelMapper postModelMapper)
    {
        _authorRepository = authorRepository;
        _bookRepository = bookRepository;
        _postModelMapper = postModelMapper;
    }

    //### GET ###

    [HttpGet("Collection")]
    public async Task<IActionResult> GetAuthors()
    {
        var authors = await _authorRepository.GetAuthors();

        if (authors == null)
        {
            ModelState.AddModelError("Server", "Null collection");
            return BadRequest(ModelState);
        }

        if (authors.Count == 0)
        {
            return NotFound();
        }

        var authorDtos = _authorMapper.AuthorToAuthorDto(authors);

        return Ok(authorDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAuthor([FromRoute] string id)
    {
        if (!Ulid.TryParse(id, out Ulid ulid))
        {
            ModelState.AddModelError("ID", "Invalid id");
            return BadRequest(ModelState);
        }

        var author = await _authorRepository.GetAuthor(ulid);

        if (author == null)
        {
            return NotFound();
        }

        var authorDto = _authorMapper.AuthorToAuthorDto(author);

        return Ok(authorDto);
    }

    [HttpGet("Name/{name}")]
    public async Task<IActionResult> GetAuthorByName([FromRoute] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            ModelState.AddModelError("Name", "Should provide name");
            return BadRequest(ModelState);
        }

        var authors = await _authorRepository.GetAuthorsByName(name);

        if (authors == null)
        {
            ModelState.AddModelError("Server", "Null collection");
            return BadRequest(ModelState);
        }

        if (authors.Count == 0)
        {
            return NotFound();
        }

        var authorDto = _authorMapper.AuthorToAuthorDto(authors);

        return Ok(authorDto);
    }

    //### POST ####

    [HttpPost]
    public async Task<IActionResult> CreateAuthor([FromBody] AuthorPostDto authorDto)
    {
        if (authorDto == null)
        {
            ModelState.AddModelError("Entity", "Should provide item for update");
            return BadRequest(ModelState);
        }

        if (string.IsNullOrWhiteSpace(authorDto.Name))
        {
            ModelState.AddModelError("Name", "Should provide name");
            return BadRequest(ModelState);
        }

        var author = await _postModelMapper.AuthorPostDtoToBook(authorDto, ModelState, _bookRepository);

        if (!await _authorRepository.CreateAuthor(author))
        {
            return StatusCode(500);
        }

        return Ok();
    }

    //### PUT ###

    [HttpPut]
    public async Task<IActionResult> UpdateAuthor([FromBody] AuthorPatchDto authorPatchDto)
    {
        if (authorPatchDto == null)
        {
            ModelState.AddModelError("Entity", "Should provide item for update");
            return BadRequest(ModelState);
        }

        if (!Ulid.TryParse(authorPatchDto.Id, out Ulid ulid))
        {
            ModelState.AddModelError("ID", "Invalid id");
            return BadRequest(ModelState);
        }

        var author = await _authorRepository.GetAuthor(ulid);

        if (author == null)
        {
            return NotFound(ModelState);
        }

        if (authorPatchDto.Name != null)
        {
            author.Name = authorPatchDto.Name;
        }

        if (authorPatchDto.BookIds != null && authorPatchDto.BookIds.Length > 0)
        {
            List<Ulid> newBookUlids = new();

            for (int i = 0; i < authorPatchDto.BookIds!.Length; i++)
            {
                if (Ulid.TryParse(authorPatchDto.BookIds[i], out Ulid id))
                    newBookUlids.Add(id);
                else
                    ModelState.AddModelError("ID", $"Invalid book id {i}");
            }

            if (newBookUlids.Count != 0)
            {
                author.Books = await _bookRepository.GetBooks(newBookUlids) as List<Book>;
            }
        }
        else
            author.Books = null;

        if (!await _authorRepository.UpdateAuthor(author))
        {
            return StatusCode(500);
        }

        return Ok(ModelState);
    }

    //### DELETE ###

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveAuthor([FromRoute] string id)
    {
        if (!Ulid.TryParse(id, out Ulid ulid))
        {
            ModelState.AddModelError("ID", "Invalid id");
            return BadRequest(ModelState);
        }

        var author = await _authorRepository.GetAuthor(ulid);

        if (author == null)
        {
            return NotFound();
        }

        if (!await _authorRepository.DeleteAuthor(author))
        {
            return StatusCode(500);
        }

        return Ok();
    }
}
