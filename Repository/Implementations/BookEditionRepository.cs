using LibraryV2.DataContext;
using LibraryV2.Models;
using LibraryV2.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryV2.Repository.Implementations
{
    public class BookEditionRepository : IBookEditionRepository
    {
        private readonly LibraryDataContext _context;

        public BookEditionRepository(LibraryDataContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateBookEdition(BookEdition edition)
        {
            _context.BookEditions.Add(edition);
            return await Save();
        }        

        public async Task<BookEdition> GetBookEdition(Ulid id)
        {
            return await _context.BookEditions
                .Where(e => e.Id == id)
                .Include(e => e.Books)
                .FirstOrDefaultAsync()!;
        }

        public async Task<IReadOnlyList<BookEdition>> GetBookEditions()
        {
            return await _context.BookEditions
                .Include(e => e.Books)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<BookEdition>> GetBookEditions(ICollection<Ulid> ids)
        {
            return await _context.BookEditions.Where(be => ids.Contains(be.Id)).ToListAsync();
        }

        public async Task<IReadOnlyList<BookEdition>> GetBookEditionsByName(string name)
        {
            return await _context.BookEditions
                .Where(e => EF.Functions.Like(e.Name, $"%{name}%"))
                .Take(100)
                .Include(e => e.Books)
                .ToListAsync()!;
        }        

        public async Task<bool> UpdateBookEdition(BookEdition edition)
        {
            _context.BookEditions.Update(edition);
            return await Save();
        }

        public async Task<bool> DeleteBookEdition(BookEdition edition)
        {
            _context.BookEditions.Remove(edition);
            return await Save();
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
