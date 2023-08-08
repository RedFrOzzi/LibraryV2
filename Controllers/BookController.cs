using LibraryV2.Dto.PatchDto;
using LibraryV2.Dto.PostDto;
using LibraryV2.Mapper;
using LibraryV2.Models;
using LibraryV2.Repository.Interfaces;
using LibraryV2.Survices;
using LibraryV2.Survices.BookSurvices;
using Microsoft.AspNetCore.Mvc;

namespace LibraryV2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookController : ControllerBase
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IBookEditionRepository _bookEditionRepository;
    private readonly IModelMapperService _postModelMapper;
    private readonly IImageManager _imageManager;
    private readonly IBookCoverRepository _bookCoverRepository;
    private readonly BookMapper _bookMapper = new();

    public BookController(IBookRepository bookRepository,
                          IAuthorRepository authorRepository,
                          IBookEditionRepository bookEditionRepository,
                          IModelMapperService postModelMapper,
                          IImageManager imageManager,
                          IBookCoverRepository bookCoverRepository)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _bookEditionRepository = bookEditionRepository;
        _postModelMapper = postModelMapper;
        _imageManager = imageManager;
        _bookCoverRepository = bookCoverRepository;
    }

    //### GET ###

    [HttpGet("collection/{page:int}&{pageSize:int}")]
    public async Task<IActionResult> GetBooks([FromRoute] int page, [FromRoute] int pageSize)
    {
        var books = await _bookRepository.GetBooks(page, pageSize);

        if (books == null)
        {
            ModelState.AddModelError("Server", "Null collection");
            return BadRequest(ModelState);
        }

        if (books.TotalCount == 0)
        {
            return NotFound();
        }

        var bookDtos = _bookMapper.BookToBookDto(books.Items);

        for (int i = 0; i < bookDtos.Count; i++)
        {
            byte[]? array = null;
            try
            {
                array = books.Items[i].BookCover?.ImageData;
            }
            catch (Exception)
            {
                ModelState.AddModelError("Entity", $"Can not download image for book '{books.Items[i].Title}'");
            }
            bookDtos[i].Image = array;
        }

        return Ok(bookDtos);
    }

    [HttpGet("collection/book/{id}")]
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


        byte[]? array = null;
        try
        {
            array = book.BookCover?.ImageData;
        }
        catch (Exception)
        {
            ModelState.AddModelError("Entity", "Can not download image");
        }
        bookDto.Image = array;

        return Ok(bookDto);
    }

        [HttpGet("title/{title}/{page:int}&{pageSize:int}")]
    public async Task<IActionResult> GetBookByTitle([FromRoute] string title, [FromRoute] int page, [FromRoute] int pageSize)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            ModelState.AddModelError("Title", "Should provide title");
            return BadRequest(ModelState);
        }

        var books = await _bookRepository.GetBooksByTitle(title, page, pageSize);

        if (books == null)
        {
            ModelState.AddModelError("Server", "Null collection");
            return BadRequest(ModelState);
        }

        if (books.TotalCount == 0)
        {
            return NotFound();
        }

        var bookDtos = _bookMapper.BookToBookDto(books.Items);

        for (int i = 0; i < bookDtos.Count; i++)
        {
            byte[]? array = null;
            try
            {
                array = books.Items[i].BookCover?.ImageData;
            }
            catch (Exception)
            {
                ModelState.AddModelError("Entity", $"Can not download image for book '{books.Items[i].Title}'");
            }
            bookDtos[i].Image = array;
        }

        return Ok(bookDtos);
    }

    //### POST ####

    [HttpPost]
    public async Task<IActionResult> CreateBook([FromForm] BookPostDto bookDto)
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

        BookCover? bookCover;
        try
        {
            bookCover = (bookDto.BookCover is not null) ? await _imageManager.PostFileAsync(bookDto.BookCover, book) : null;
        }
        catch (Exception)
        {
            ModelState.AddModelError("Entity", "Can not upload image");
            return BadRequest(ModelState);
        }

        book.BookCover = bookCover;

        if (!await _bookRepository.CreateBook(book))
        {
            return StatusCode(500);
        }

        return Ok();
    }

    //### PUT ###

    [HttpPut]
    public async Task<IActionResult> UpdateBook([FromForm] BookPatchDto bookDto)
    {
        if (bookDto == null)
        {
            ModelState.AddModelError("Entity", "Should provide item for update");
            return BadRequest(ModelState);
        }

        if (!Ulid.TryParse(bookDto.Id, out Ulid ulid))
        {
            ModelState.AddModelError("ID", "Invalid id");
            return BadRequest(ModelState);
        }

        var book = await _bookRepository.GetBook(ulid);

        if (book == null)
        {
            return NotFound(ModelState);
        }

        var mappedBook = await _postModelMapper.BookPatchDtoToBook(book, bookDto, ModelState, _imageManager,
                                                                   _bookEditionRepository, _authorRepository,
                                                                   _bookCoverRepository);

        if (!await _bookRepository.UpdateBook(mappedBook))
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

        if (book.BookCover is not null)
        {
            if (!await _bookCoverRepository.DeleteBookCover(book.BookCover))
            {
                return StatusCode(500);
            }
        }

        if (!await _bookRepository.DeleteBook(book))
        {
            return StatusCode(500);
        }

        return Ok();
    }
}
