using LibraryV2.DataContext;
using LibraryV2.Dto;
using LibraryV2.Models;
using LibraryV2.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryV2.Repository.Implementations;

public class AuthorRepository : IAuthorRepository
{
    private readonly LibraryDataContext _context;

    public AuthorRepository(LibraryDataContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateAuthor(Author author)
    {
        _context.Authors.Add(author);
        return await Save();
    }

    public async Task<Author> GetAuthor(Ulid id)
    {
        return await _context.Authors
            .Where(a => a.Id == id)
            .Include(a => a.Books)
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<Author>> GetAuthors()
    {
        return await _context.Authors
            .Include(a => a.Books)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Author>> GetAuthors(ICollection<Ulid> ids)
    {
        return await _context.Authors.Where(a => ids.Contains(a.Id)).ToListAsync();
    }

    public async Task<IReadOnlyList<Author>> GetAuthorsByName(string name)
    {
        return await _context.Authors
            .Where(a => EF.Functions.Like(a.Name, $"%{name}%"))
            .Take(100)
            .Include(a => a.Books)
            .ToListAsync()!;
    }

    public async Task<bool> UpdateAuthor(Author author)
    {
        _context.Authors.Update(author);
        return await Save();
    }

    public async Task<bool> DeleteAuthor(Author author)
    {
        _context.Authors.Remove(author);
        return await Save();
    }

    public async Task<bool> Save()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
