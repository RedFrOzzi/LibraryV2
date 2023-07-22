using LibraryV2.DataContext;
using LibraryV2.Models;
using LibraryV2.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryV2.Repository.Implementations
{
    public class ReaderRepository : IReaderRepository
    {
        private readonly LibraryDataContext _context;

        public ReaderRepository(LibraryDataContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateReader(Reader reader)
        {
            _context.Readers.Add(reader);
            return await Save();
        }        

        public async Task<Reader> GetReader(Ulid id)
        {
            return await _context.Readers
                .Where(x => x.Id == id)
                .Include(x => x.BorrowedBooks)
                .FirstOrDefaultAsync()!;
        }
        
        public async Task<Reader> GetReader(string login)
        {
            return await _context.Readers
                .Where(x => x.Login == login)
                .Include(x => x.BorrowedBooks)
                .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<Reader>> GetReaders()
        {
            return await _context.Readers
                .Include(x => x.BorrowedBooks)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Reader>> GetReaders(ICollection<Ulid> ids)
        {
            return await _context.Readers.Where(r => ids.Contains(r.Id)).ToListAsync();
        }

        public async Task<IReadOnlyList<Reader>> GetReadersByName(string name)
        {
            return await _context.Readers
                .Where(b => EF.Functions.Like(b.Name, $"%{name}%"))
                .Take(100)
                .Include(x => x.BorrowedBooks)
                .ToListAsync()!;
        }        

        public async Task<bool> UpdateReader(Reader reader)
        {
            _context.Readers.Update(reader);
            return await Save();
        }

        public async Task<bool> DeleteReader(Reader reader)
        {
            _context.Readers.Remove(reader);
            return await Save();
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
