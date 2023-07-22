using LibraryV2.Dto;
using LibraryV2.Dto.PostDto;
using LibraryV2.Models;
using LibraryV2.Repository.Implementations;
using LibraryV2.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LibraryV2.Mapper;

public class PostModelMapper : IPostModelMapper
{
    public async Task<Author> AuthorPostDtoToBook(AuthorPostDto authorPostDto, ModelStateDictionary modelState, IBookRepository bookRepository)
    {
        var author = new Author()
        {
            Name = authorPostDto.Name
        };

        if (authorPostDto.BookIds != null && authorPostDto.BookIds.Length > 0)
        {
            List<Ulid> newBookIds = new();

            for (int i = 0; i < authorPostDto.BookIds.Length; i++)
            {
                if (Ulid.TryParse(authorPostDto.BookIds[i], out Ulid id))
                    newBookIds.Add(id);
                else
                    modelState.AddModelError("ID", $"Invalid author id {i}");
            }

            if (newBookIds.Count != 0)
            {
                author.Books = await bookRepository.GetBooks(newBookIds) as List<Book>;
            }
        }
        else
            author.Books = null;

        return author;
    }

    public async Task<BookEdition> BookEditionPostDtoToBook(BookEditionPostDto bookEditionPostDto,
                                                      ModelStateDictionary modelState,
                                                      IBookRepository bookRepository)
    {
        var edition = new BookEdition()
        {
            Name = bookEditionPostDto.Name
        };

        if (bookEditionPostDto.BookIds != null && bookEditionPostDto.BookIds.Length > 0)
        {
            List<Ulid> newBookIds = new();

            for (int i = 0; i < bookEditionPostDto.BookIds.Length; i++)
            {
                if (Ulid.TryParse(bookEditionPostDto.BookIds[i], out Ulid id))
                    newBookIds.Add(id);
                else
                    modelState.AddModelError("ID", $"Invalid author id {i}");
            }

            if (newBookIds.Count != 0)
            {
                edition.Books = await bookRepository.GetBooks(newBookIds) as List<Book>;
            }
        }
        else
            edition.Books = null;

        return edition;
    }

    public async Task<Book> BookPostDtoToBook(BookPostDto bookPostDto,
                                              ModelStateDictionary modelState,
                                              IBookEditionRepository bookEditionRepository,
                                              IAuthorRepository authorRepository)
    {
        var book = new Book()
        {
            Title = bookPostDto.Title
        };

        if (bookPostDto.AuthorIds != null && bookPostDto.AuthorIds.Length > 0)
        {
            List<Ulid> newAuthorIds = new();

            for (int i = 0; i < bookPostDto.AuthorIds.Length; i++)
            {
                if (Ulid.TryParse(bookPostDto.AuthorIds[i], out Ulid id))
                    newAuthorIds.Add(id);
                else
                    modelState.AddModelError("ID", $"Invalid author id {i}");
            }

            if (newAuthorIds.Count != 0)
            {
                book.Authors = await authorRepository.GetAuthors(newAuthorIds) as List<Author>;
            }
        }
        else
            book.Authors = null;

        if (bookPostDto.EditionId != null && Ulid.TryParse(bookPostDto.EditionId, out Ulid ulid))
        {
            book.Edition = await bookEditionRepository.GetBookEdition(ulid);
        }

        if (bookPostDto.ReleaseDate != null)
        {
            book.ReleaseDate = bookPostDto.ReleaseDate;
        }

        return book;
    }

    public Reader ReaderPostDtoToReader(ReaderPostDto readerPostDto, ModelStateDictionary modelState)
    {
        var reader = new Reader()
        {
            Name = readerPostDto.Name ?? string.Empty,
            Login = readerPostDto.Login
        };

        return reader;
    }
}
