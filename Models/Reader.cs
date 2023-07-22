using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryV2.Models;

public class Reader
{
    public Reader()
    {
        Id = Ulid.NewUlid();
    }
    [Key]
    public Ulid Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Login { get; set; }
    [Required]
    public byte[] PasswordHash { get; set; }
    public ICollection<Book>? BorrowedBooks { get; set; } = new List<Book>();
}
