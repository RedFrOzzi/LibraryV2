using LibraryV2.Models;

namespace LibraryV2.Dto;

public class ReaderDto
{
    public Ulid? Id { get; set; }
    public string? Name { get; set; }
    public string? Login { get; set; }
    public ICollection<Book>? BorrowedBooks { get; set; }
}
