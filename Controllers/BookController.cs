using LibraryV2.Dto;
using LibraryV2.Dto.PatchDto;
using LibraryV2.Dto.PostDto;
using LibraryV2.Mapper;
using LibraryV2.Models;
using LibraryV2.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection.Metadata.Ecma335;

namespace LibraryV2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookController : ControllerBase
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IBookEditionRepository _bookEditionRepository;
    private readonly IPostModelMapper _postModelMapper;
    private readonly BookMapper _bookMapper = new();

    public BookController(IBookRepository bookRepository,
                          IAuthorRepository authorRepository,
                          IBookEditionRepository bookEditionRepository,
                          IPostModelMapper postModelMapper)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _bookEditionRepository = bookEditionRepository;
        _postModelMapper = postModelMapper;
    }

    //### GET ###

    [HttpGet("Collection")]
    public async Task<IActionResult> GetBooks()
    {
        var books = await _bookRepository.GetBooks();

        if (books == null)
        {
            ModelState.AddModelError("Server", "Null collection");
            return BadRequest(ModelState);
        }

        if (books.Count == 0)
        {
            return NotFound();
        }

        var bookDtos = _bookMapper.BookToBookDto(books);

        return Ok(bookDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBook([FromRoute] string id)
    {
        if (!Ulid.TryParse(id, out Ulid ulid))
        {
            ModelState.AddModelError("ID", "Invalid id");
            return BadRequest(ModelState);
        }

        var book = await _bookRepository.GetBook(ulid);

        if (book == null)
        {
            return NotFound();
        }

        var bookDto = _bookMapper.BookToBookDto(book);

        return Ok(bookDto);
    }

    [HttpGet("Title/{title}")]
    public async Task<IActionResult> GetBookByTitle([FromRoute] string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            ModelState.AddModelError("Title", "Should provide title");
            return BadRequest(ModelState);
        }

        var books = await _bookRepository.GetBooksByTitle(title);

        if (books == null)
        {
            ModelState.AddModelError("Server", "Null collection");
            return BadRequest(ModelState);
        }

        if (books.Count == 0)
        {
            return NotFound();
        }

        var bookDtos = _bookMapper.BookToBookDto(books);

        return Ok(bookDtos);
    }

    //### POST ####

    [HttpPost]
    public async Task<IActionResult> CreateBook([FromBody] BookPostDto bookDto)
    {
        if (bookDto == null)
        {
            ModelState.AddModelError("Entity", "Should provide item for update");
            return BadRequest(ModelState);
        }

        if (string.IsNullOrWhiteSpace(bookDto.Title))
        {
            ModelState.AddModelError("Title", "Should provide title");
            return BadRequest(ModelState);
        }

        var book = await _postModelMapper.BookPostDtoToBook(bookDto, ModelState, _bookEditionRepository, _authorRepository);

        if (!await _bookRepository.CreateBook(book))
        {
            return StatusCode(500);
        }

        return Ok();
    }

    //### PUT ###

    [HttpPut]
    public async Task<IActionResult> UpdateBook([FromBody] BookPatchDto bookPatchDto)
    {
        if (bookPatchDto == null)
        {
            ModelState.AddModelError("Entity", "Should provide item for update");
            return BadRequest(ModelState);
        }

        if (!Ulid.TryParse(bookPatchDto.Id, out Ulid ulid))
        {
            ModelState.AddModelError("ID", "Invalid id");
            return BadRequest(ModelState);
        }

        var book = await _bookRepository.GetBook(ulid);

        if (book == null)
        {
            return NotFound(ModelState);
        }

        if (bookPatchDto.Title != null)
        {
            book.Title = bookPatchDto.Title;
        }

        if (bookPatchDto.ReleaseDate != null && bookPatchDto.ReleaseDate != DateTime.MinValue)
        {
            book.ReleaseDate = bookPatchDto.ReleaseDate;
        }

        if (bookPatchDto.AuthorIds != null && bookPatchDto.AuthorIds.Length > 0)
        {
            List<Ulid> newAuthorIds = new();

            for (int i = 0; i < bookPatchDto.AuthorIds!.Length; i++)
            {
                if (Ulid.TryParse(bookPatchDto.AuthorIds[i], out Ulid id))
                    newAuthorIds.Add(id);
                else
                    ModelState.AddModelError("ID", $"Invalid author id {i}");
            }

            if (newAuthorIds.Count != 0)
            {
                book.Authors = await _authorRepository.GetAuthors(newAuthorIds) as List<Author>;
            }
        }
        else
            book.Authors = null;

        if (bookPatchDto.EditionId != null)
        {
            if (Ulid.TryParse(bookPatchDto.EditionId, out Ulid id))
                book.Edition = await _bookEditionRepository.GetBookEdition(id);
            else
                ModelState.AddModelError("ID", "Invalid book edition id");
        }
        else
            book.Edition = null;

        if (!await _bookRepository.UpdateBook(book))
        { 
            return StatusCode(500);
        }

        if (ModelState.IsValid)
            return NoContent();
        else
            return Ok(ModelState);
    }

    //### DELETE ###

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveBook([FromRoute] string id)
    {
        if (!Ulid.TryParse(id, out Ulid ulid))
        {
            ModelState.AddModelError("ID", "Invalid id");
            return BadRequest(ModelState);
        }

        var book = await _bookRepository.GetBook(ulid);

        if (book == null)
        {
            return NotFound();
        }

        if (!await _bookRepository.DeleteBook(book))
        {
            return StatusCode(500);
        }

        return Ok();
    }
}
