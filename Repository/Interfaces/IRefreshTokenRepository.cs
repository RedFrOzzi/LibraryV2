using LibraryV2.Models;

namespace LibraryV2.Repository.Interfaces;

public interface IRefreshTokenRepository
{
    Task<bool> CreateRefreshToken(RefreshToken refreshToken);
    Task<RefreshToken> GetRefreshToken(Ulid ulid);
    Task<RefreshToken> GetRefreshToken(Reader reader);
    Task<bool> RefreshTokenExist(Reader reader);
    Task<bool> UpdateRefreshToken(RefreshToken refreshToken);
    Task<bool> DeleteRefreshToken(RefreshToken refreshToken);
    Task<bool> DeleteAllRefreshTokens();
    Task<bool> Save();
}
