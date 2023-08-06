using System.ComponentModel.DataAnnotations;

namespace LibraryV2.Models;

public class Book
{
    public Book()
    {
        Id = Ulid.NewUlid();
    }
    [Key]
    public Ulid Id { get; set; }
    [Required]
    public string Title { get; set; }
    public BookCover? BookCover { get; set; }
    public ICollection<Author>? Authors { get; set; } = new List<Author>();
    public BookEdition? Edition { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public Reader? CurrentReader { get; set; }
}
