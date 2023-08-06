
using LibraryV2.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryV2.Dto;

public class BookDto
{
    public Ulid? Id { get; set; }
    public string? Title { get; set; }
    public FileContentResult? Image { get; set; }
    public ICollection<Author>? Authors { get; set; }
    public BookEdition? Edition { get; set; }
    public DateTime? ReleaseDate { get; set; }
}
