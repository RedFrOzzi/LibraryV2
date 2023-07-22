using LibraryV2.Models;

namespace LibraryV2.Repository.Interfaces;

public interface IAuthorRepository
{
    Task<bool> CreateAuthor(Author author);
    Task<Author> GetAuthor(Ulid id);
    Task<IReadOnlyList<Author>> GetAuthors();
    Task<IReadOnlyList<Author>> GetAuthors(ICollection<Ulid> ids);
    Task<IReadOnlyList<Author>> GetAuthorsByName(string name);
    Task<bool> UpdateAuthor(Author author);
    Task<bool> DeleteAuthor(Author author);
    Task<bool> Save();
}
