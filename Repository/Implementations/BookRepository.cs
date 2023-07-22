using LibraryV2.DataContext;
using LibraryV2.Models;
using LibraryV2.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryV2.Repository.Implementations
{
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
                .FirstOrDefaultAsync()!;
        }

        public async Task<IReadOnlyList<Book>> GetBooks()
        {
            return await _context.Books
                .Include(b => b.Authors)
                .Include(b => b.Edition)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Book>> GetBooks(ICollection<Ulid> ids)
        {
            return await _context.Books.Where(b => ids.Contains(b.Id)).ToListAsync();
        }

        public async Task<IReadOnlyList<Book>> GetBooksByTitle(string title)
        {
            return await _context.Books
                .Where(b => EF.Functions.Like(b.Title, $"%{title}%"))
                .Take(100)
                .Include(b => b.Authors)
                .Include(b => b.Edition)
                .ToListAsync()!;
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
}
