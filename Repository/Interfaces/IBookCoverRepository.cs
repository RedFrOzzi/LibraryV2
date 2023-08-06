using LibraryV2.Models;

namespace LibraryV2.Repository.Interfaces;

public interface IBookCoverRepository
{
    Task<BookCover> CreateBookCover(BookCover bookCover);
    Task<BookCover> GetBookCover(Ulid id);
    Task<bool> Save();
}
