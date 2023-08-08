using LibraryV2.Dto.PatchDto;
using LibraryV2.Dto.PostDto;
using LibraryV2.Models;
using LibraryV2.Repository.Interfaces;
using LibraryV2.Survices.BookSurvices;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LibraryV2.Survices;

public class ModelMapperService : IModelMapperService
{
    public async Task<Author> AuthorPostDtoToAuthor(AuthorPostDto authorPostDto,
                                                    ModelStateDictionary modelState,
                                                    IBookRepository bookRepository)
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

    public async Task<BookEdition> BookEditionPostDtoToBookEdition(BookEditionPostDto bookEditionPostDto,
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

    public async Task<Book> BookPatchDtoToBook(Book book,
                                               BookPatchDto bookPatchDto,
                                               ModelStateDictionary modelState,
                                               IImageManager imageManager,
                                               IBookEditionRepository bookEditionRepository,
                                               IAuthorRepository authorRepository,
                                               IBookCoverRepository bookCoverRepository)
    {
        if (bookPatchDto.Title != null)
        {
            book.Title = bookPatchDto.Title;
        }

        if (book.BookCover != null && bookPatchDto.BookCover != null)
        {
            await bookCoverRepository.DeleteBookCover(book.BookCover);

            try
            {
                book.BookCover = bookPatchDto.BookCover is not null ? await imageManager.PostFileAsync(bookPatchDto.BookCover, book) : null;
            }
            catch (Exception)
            {
                modelState.AddModelError("Entity", "Can not upload image");
            }
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
                    modelState.AddModelError("ID", $"Invalid author id {i}");
            }

            if (newAuthorIds.Count != 0)
            {
                book.Authors = await authorRepository.GetAuthors(newAuthorIds) as List<Author>;
            }
        }
        else
            book.Authors = null;

        if (bookPatchDto.EditionId != null)
        {
            if (Ulid.TryParse(bookPatchDto.EditionId, out Ulid id))
                book.Edition = await bookEditionRepository.GetBookEdition(id);
            else
                modelState.AddModelError("ID", "Invalid book edition id");
        }
        else
            book.Edition = null;

        return book;
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
