using LibraryV2.DataContext;
using LibraryV2.Migrations;
using LibraryV2.Models;
using LibraryV2.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace LibraryV2.Repository.Implementations;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly LibraryDataContext _context;

    public RefreshTokenRepository(LibraryDataContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateRefreshToken(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Add(refreshToken);
        return await Save();
    }

    public async Task<RefreshToken> GetRefreshToken(Ulid ulid)
    {
        return await _context.RefreshTokens.AsNoTracking().Where(rf => rf.Id == ulid).FirstOrDefaultAsync();
    }

    public async Task<RefreshToken> GetRefreshToken(Reader reader)
    {
        return await _context.RefreshTokens.AsNoTracking().Where(rf => rf.Reader == reader).FirstOrDefaultAsync();
    }

    public async Task<bool> RefreshTokenExist(Reader reader)
    {
        return await _context.RefreshTokens.AsNoTracking().Where(rf => rf.Reader == reader).AnyAsync();
    }

    public async Task<bool> UpdateRefreshToken(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Update(refreshToken);
        return await Save();
    }

    public async Task<bool> DeleteRefreshToken(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Remove(refreshToken);
        return await Save();
    }

    public async Task<bool> DeleteAllRefreshTokens()
    {
        if (!_context.RefreshTokens.AsNoTracking().Any())
            return true;

        _context.RefreshTokens.RemoveRange(_context.RefreshTokens.AsQueryable());

        return await Save();
    }

    public async Task<bool> Save()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
