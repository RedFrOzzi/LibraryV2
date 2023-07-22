using LibraryV2.Models;

namespace LibraryV2.Repository.Interfaces
{
    public interface IBookEditionRepository
    {
        Task<bool> CreateBookEdition(BookEdition edition);
        Task<BookEdition> GetBookEdition(Ulid id);
        Task<IReadOnlyList<BookEdition>> GetBookEditions();
        Task<IReadOnlyList<BookEdition>> GetBookEditions(ICollection<Ulid> ids);
        Task<IReadOnlyList<BookEdition>> GetBookEditionsByName(string name);
        Task<bool> UpdateBookEdition(BookEdition edition);
        Task<bool> DeleteBookEdition(BookEdition edition);
        Task<bool> Save();
    }
}
