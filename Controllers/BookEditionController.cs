using LibraryV2.Dto;
using LibraryV2.Dto.PostDto;
using LibraryV2.Mapper;
using LibraryV2.Models;
using LibraryV2.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryV2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookEditionController : ControllerBase
{
    private readonly IBookEditionRepository _bookEditionRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IPostModelMapper _postModelMapper;
    private readonly BookEditionMapper _bookEditionMapper = new();

    public BookEditionController(IBookEditionRepository bookEditionRepository,
                                 IBookRepository bookRepository,
                                 IPostModelMapper postModelMapper)
    {
        _bookEditionRepository = bookEditionRepository;
        _bookRepository = bookRepository;
        _postModelMapper = postModelMapper;
    }

    //### GET ###

    [HttpGet("collection")]
    public async Task<IActionResult> GetEditions()
    {
        var editions = await _bookEditionRepository.GetBookEditions();

        if (editions == null)
        {
            ModelState.AddModelError("Server", "Null collection");
            return BadRequest(ModelState);
        }

        if (editions.Count == 0)
        {
            return NotFound();
        }

        var editionDtos = _bookEditionMapper.EditionToEditionDto(editions);

        return Ok(editionDtos);
    }

    [HttpGet("collection/{id}")]
    public async Task<IActionResult> GetEdition([FromRoute] string id)
    {
        if (!Ulid.TryParse(id, out Ulid ulid))
        {
            ModelState.AddModelError("ID", "Invalid id");
            return BadRequest(ModelState);
        }

        var edition = await _bookEditionRepository.GetBookEdition(ulid);

        if (edition == null)
        {
            return NotFound();
        }

        var authorDto = _bookEditionMapper.EditionToEditionDto(edition);

        return Ok(authorDto);
    }

    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetEditionsByName([FromRoute] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            ModelState.AddModelError("Name", "Should provide name");
            return BadRequest(ModelState);
        }

        var editions = await _bookEditionRepository.GetBookEditionsByName(name);

        if (editions == null)
        {
            ModelState.AddModelError("Server", "Null collection");
            return BadRequest(ModelState);
        }

        if (editions.Count == 0)
        {
            return NotFound();
        }

        var editionDtos = _bookEditionMapper.EditionToEditionDto(editions);

        return Ok(editionDtos);
    }

    //### POST ####

    [HttpPost]
    public async Task<IActionResult> CreateEditions([FromForm] BookEditionPostDto editionDto)
    {
        if (editionDto == null)
        {
            ModelState.AddModelError("Entity", "Should provide item for update");
            return BadRequest(ModelState);
        }

        if (string.IsNullOrWhiteSpace(editionDto.Name))
        {
            ModelState.AddModelError("Name", "Should provide name");
            return BadRequest(ModelState);
        }

        var edition = await _postModelMapper.BookEditionPostDtoToBookEdition(editionDto, ModelState, _bookRepository);

        if (!await _bookEditionRepository.CreateBookEdition(edition))
        {
            return StatusCode(500);
        }

        return Ok();
    }

    //### PATCH ###

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateEdition([FromRoute] string id, [FromBody] BookEditionDto editionDto)
    {
        if (editionDto == null)
        {
            ModelState.AddModelError("Entity", "Should provide item for update");
            return BadRequest(ModelState);
        }

        if (!Ulid.TryParse(id, out Ulid ulid))
        {
            ModelState.AddModelError("ID", "Invalid id");
            return BadRequest(ModelState);
        }

        var edition = await _bookEditionRepository.GetBookEdition(ulid);

        if (edition == null)
        {
            return NotFound();
        }

        if (editionDto.Name != null)
        {
            edition.Name = editionDto.Name;
        }

        if (editionDto.Books != null)
        {
            edition.Books = editionDto.Books;
        }

        if (!await _bookEditionRepository.UpdateBookEdition(edition))
        {
            return StatusCode(500);
        }

        return Ok();
    }

    //### DELETE ###

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveEdition([FromRoute] string id)
    {
        if (!Ulid.TryParse(id, out Ulid ulid))
        {
            ModelState.AddModelError("ID", "Invalid id");
            return BadRequest(ModelState);
        }

        var edition = await _bookEditionRepository.GetBookEdition(ulid);

        if (edition == null)
        {
            return NotFound();
        }

        if (!await _bookEditionRepository.DeleteBookEdition(edition))
        {
            return StatusCode(500);
        }

        return Ok();
    }
}
