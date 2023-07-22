
using LibraryV2.Models;

namespace LibraryV2.Dto;

public class BookEditionDto
{
    public Ulid? Id { get; set; }
    public string? Name { get; set; }
    public ICollection<Book>? Books { get; set; }
}
