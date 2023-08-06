using LibraryV2.Models;
using LibraryV2.Utilities;

namespace LibraryV2.Repository.Interfaces;

public interface IBookRepository
{
    Task<bool> CreateBook(Book book);
    Task<Book> GetBook(Ulid id);
    Task<PagedList<Book>> GetBooks(int page, int pageSize);
    Task<IReadOnlyList<Book>> GetBooks(ICollection<Ulid> ids);
    Task<IReadOnlyList<Book>> GetBooksByTitle(string title);
    Task<bool> UpdateBook(Book book);
    Task<bool> DeleteBook(Book book);
    Task<bool> Save();
}
