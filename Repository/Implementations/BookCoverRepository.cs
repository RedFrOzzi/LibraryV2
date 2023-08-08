using LibraryV2.DataContext;
using LibraryV2.Models;
using LibraryV2.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryV2.Repository.Implementations;

public class BookCoverRepository : IBookCoverRepository
{
    private readonly LibraryDataContext _context;

    public BookCoverRepository(LibraryDataContext context)
    {
        _context = context;
    }

    public async Task<BookCover> CreateBookCover(BookCover bookCover)
    {
        var result = _context.BookCovers.Add(bookCover);
        await Save();
        return result.Entity;
    }

    public async Task<BookCover> GetBookCover(Ulid id)
    {
        return await _context.BookCovers
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<bool> DeleteBookCover(BookCover bookCover)
    {
        if (bookCover != null)
            _context.BookCovers.Remove(bookCover);

        return await Save();
    }

    public async Task<bool> Save()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
