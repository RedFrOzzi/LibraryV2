using LibraryV2.DataContext;
using LibraryV2.Models;
using LibraryV2.Repository.Interfaces;
using LibraryV2.Utilities;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;

namespace LibraryV2.Repository.Implementations;

public class BookRepository : IBookRepository
{
    private readonly LibraryDataContext _context;

    public BookRepository(LibraryDataContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateBook(Book book)
    {
        _context.Books.Add(book);
        return await Save();
    }

    public async Task<Book> GetBook(Ulid id)
    {
        return await _context.Books
            .Where(b => b.Id == id)
            .Include(b => b.Authors)
            .Include(b => b.Edition)
            .Include(b => b.BookCover)
            .FirstOrDefaultAsync();
    }

    public async Task<PagedList<Book>> GetBooks(int page, int pageSize)
    {
        var books = _context.Books
            .OrderBy(b => b.Id)
            .Include(b => b.Authors)
            .Include(b => b.Edition)
            .Include(b => b.BookCover);

        return await PagedList<Book>.CreateAsync(books, page, pageSize);
    }

    public async Task<IReadOnlyList<Book>> GetBooks(ICollection<Ulid> ids)
    {
        return await _context.Books.Where(b => ids.Contains(b.Id)).ToListAsync();
    }

    public async Task<PagedList<Book>> GetBooksByTitle(string title, int page, int pageSize)
    {
        var books = _context.Books
            .Where(b => EF.Functions.Like(b.Title, $"%{title}%"))
            .OrderBy(b => b.Id)
            .Include(b => b.Authors)
            .Include(b => b.Edition);

        return await PagedList<Book>.CreateAsync(books, page, pageSize);
    }  

    public async Task<bool> UpdateBook(Book book)
    {
        _context.Books.Update(book);
        return await Save();
    }

    public async Task<bool> DeleteBook(Book book)
    {
        _context.Books.Remove(book);
        return await Save();
    }

    public async Task<bool> Save()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
