using LibraryV2.Models;

namespace LibraryV2.Repository.Interfaces
{
    public interface IReaderRepository
    {
        Task<bool> CreateReader(Reader reader);
        Task<Reader> GetReader(Ulid id);
        Task<Reader> GetReader(string login);
        Task<IReadOnlyList<Reader>> GetReaders();
        Task<IReadOnlyList<Reader>> GetReaders(ICollection<Ulid> ids);
        Task<IReadOnlyList<Reader>> GetReadersByName(string name);
        Task<bool> UpdateReader(Reader reader);
        Task<bool> DeleteReader(Reader reader);
        Task<bool> Save();
    }
}
