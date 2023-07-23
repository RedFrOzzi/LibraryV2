using LibraryV2.Dto.PatchDto;
using LibraryV2.Mapper;
using LibraryV2.Models;
using LibraryV2.Repository.Interfaces;
using LibraryV2.Survices.ReaderSurvice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryV2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReaderController : ControllerBase
{
    private readonly IReaderRepository _readerRepository;
    private readonly IBookRepository _bookRepository;
    private readonly ReaderMapper _readerMapper = new();

    public ReaderController(IReaderRepository readerRepository,
                            IBookRepository bookRepository)
    {
        _readerRepository = readerRepository;
        _bookRepository = bookRepository;
    }

    //### GET ###

    [HttpGet("collection"), Authorize]
    public async Task<IActionResult> GetReaders()
    {
        var readers = await _readerRepository.GetReaders();

        if (readers == null)
        {
            ModelState.AddModelError("Server", "Null collection");
            return BadRequest(ModelState);
        }

        if (readers.Count == 0)
        {
            return NotFound();
        }

        var bookDtos = _readerMapper.ReaderToReaderDto(readers);

        return Ok(bookDtos);
    }

    [HttpGet("collection/{id}")]
    public async Task<IActionResult> GetReader([FromRoute] string id)
    {
        if (!Ulid.TryParse(id, out Ulid ulid))
        {
            ModelState.AddModelError("ID", "Invalid id");
            return BadRequest(ModelState);
        }

        var reader = await _readerRepository.GetReader(ulid);

        if (reader == null)
        {
            return NotFound();
        }

        var bookDto = _readerMapper.ReaderToReaderDto(reader);

        return Ok(bookDto);
    }

    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetReaderByName([FromRoute] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            ModelState.AddModelError("Title", "Should provide name");
            return BadRequest(ModelState);
        }

        var readers = await _readerRepository.GetReadersByName(name);

        if (readers == null)
        {
            ModelState.AddModelError("Server", "Null collection");
            return BadRequest(ModelState);
        }

        if (readers.Count == 0)
        {
            return NotFound();
        }

        var bookDtos = _readerMapper.ReaderToReaderDto(readers);

        return Ok(bookDtos);
    }

    //### PUT ###

    [HttpPut]
    public async Task<IActionResult> UpdateReader([FromBody] ReaderPatchDto readerPatchDto)
    {
        if (readerPatchDto == null)
        {
            ModelState.AddModelError("Entity", "Should provide item for update");
            return BadRequest(ModelState);
        }

        if (!Ulid.TryParse(readerPatchDto.Id, out Ulid ulid))
        {
            ModelState.AddModelError("ID", "Invalid id");
            return BadRequest(ModelState);
        }

        var reader = await _readerRepository.GetReader(ulid);

        if (reader == null)
        {
            return NotFound(ModelState);
        }

        if (readerPatchDto.Name != null)
        {
            reader.Name = readerPatchDto.Name;
        }

        if (!await _readerRepository.UpdateReader(reader))
        {
            return StatusCode(500);
        }

        return Ok(ModelState);
    }

    //### PATCH ###

    [HttpPatch("add_books")]
    public async Task<IActionResult> AddBorrowedBooks([FromBody] ReaderUpdateBooksDto readerUpdateBooksDto)
    {
        if (!Ulid.TryParse(readerUpdateBooksDto.Id, out Ulid ulid))
        {
            ModelState.AddModelError("ID", "Invalid id");
            return BadRequest(ModelState);
        }

        if (readerUpdateBooksDto.BorrowedBookIds == null || readerUpdateBooksDto.BorrowedBookIds.Length == 0)
        {
            ModelState.AddModelError("Entity", "Should provide ID's");
            return BadRequest(ModelState);
        }

        var reader = await _readerRepository.GetReader(ulid);

        if (reader == null)
        {
            return NotFound(ModelState);
        }

        var newBookIds = new List<Ulid>();

        int notParsedIdsCount = 0;

        for (int i = 0; i < readerUpdateBooksDto.BorrowedBookIds.Length; i++)
        {
            if (Ulid.TryParse(readerUpdateBooksDto.BorrowedBookIds[i], out Ulid id))
                newBookIds.Add(id);
            else
            {
                ModelState.AddModelError("ID", $"Invalid book id {i}");
                notParsedIdsCount++;
            }
        }

        if (notParsedIdsCount == readerUpdateBooksDto.BorrowedBookIds.Length)
            return BadRequest(ModelState);

        if (newBookIds.Count != 0)
        {
            reader.BorrowedBooks ??= new List<Book>();

            var newBooks = await _bookRepository.GetBooks(newBookIds);

            foreach (var book in newBooks)
            {
                reader.BorrowedBooks.Add(book);
            }
        }

        if (!await _readerRepository.UpdateReader(reader))
        {
            return StatusCode(500);
        }

        if (ModelState.IsValid)
            return NoContent();
        else
            return Ok(ModelState);
    }

    [HttpPatch("remove_books")]
    public async Task<IActionResult> RemoveBorrowedBooks([FromBody] ReaderUpdateBooksDto readerUpdateBooksDto)
    {
        if (!Ulid.TryParse(readerUpdateBooksDto.Id, out Ulid ulid))
        {
            ModelState.AddModelError("ID", "Invalid id");
            return BadRequest(ModelState);
        }

        if (readerUpdateBooksDto.BorrowedBookIds == null || readerUpdateBooksDto.BorrowedBookIds.Length == 0)
        {
            ModelState.AddModelError("Entity", "Should provide ID's");
            return BadRequest(ModelState);
        }

        var reader = await _readerRepository.GetReader(ulid);

        if (reader == null || reader.BorrowedBooks == null || reader.BorrowedBooks.Count == 0)
        {
            return NotFound(ModelState);
        }

        var booksToRemove = new List<Ulid>();

        int notParsedIdsCount = 0;

        for (int i = 0; i < readerUpdateBooksDto.BorrowedBookIds.Length; i++)
        {
            if (Ulid.TryParse(readerUpdateBooksDto.BorrowedBookIds[i], out Ulid id))
                booksToRemove.Add(id);
            else
            {
                ModelState.AddModelError("ID", $"Invalid book id {i}");
                notParsedIdsCount++;
            }
        }

        if (notParsedIdsCount == readerUpdateBooksDto.BorrowedBookIds.Length)
            return BadRequest(ModelState);

        if (booksToRemove.Count != 0)
        {
            var newBooks = await _bookRepository.GetBooks(booksToRemove);

            foreach (var book in newBooks)
            {
                reader.BorrowedBooks.Remove(book);
            }
        }

        if (!await _readerRepository.UpdateReader(reader))
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
    public async Task<IActionResult> RemoveReader([FromRoute] string id)
    {
        if (!Ulid.TryParse(id, out Ulid ulid))
        {
            ModelState.AddModelError("ID", "Invalid id");
            return BadRequest(ModelState);
        }

        var reader = await _readerRepository.GetReader(ulid);

        if (reader == null)
        {
            return NotFound();
        }

        if (!await _readerRepository.DeleteReader(reader))
        {
            return StatusCode(500);
        }

        return Ok();
    }
}
