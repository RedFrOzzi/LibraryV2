using System.ComponentModel.DataAnnotations;

namespace LibraryV2.Models;

public class BookCover
{
    public BookCover()
    {
        Id = Ulid.NewUlid();
    }
    [Key]
    public Ulid Id { get; set; }
    public string ImageName { get; set; } = string.Empty;
    public byte[] ImageData { get; set; }
}
