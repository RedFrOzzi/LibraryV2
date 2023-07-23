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
    [Required]
    public virtual int RoleType { get; set; }
    [EnumDataType(typeof(Roles))]
    public Roles Role
    {
        get
        {
            return (Roles)RoleType;
        }
        set
        {
            RoleType = (int)value;
        }
    }
    public string? RefreshToken { get; set; }
    public DateTime? TokenCreated { get; set; } = DateTime.Now;
    public DateTime? TokenExpires { get; set; } = DateTime.Now;
    public ICollection<Book>? BorrowedBooks { get; set; } = new List<Book>();
}
