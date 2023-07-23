using System.ComponentModel.DataAnnotations;

namespace LibraryV2.Models;

public class RefreshToken
{
    public RefreshToken()
    {
        Id = Ulid.NewUlid();
    }
    [Key]
    public Ulid Id { get; set; }
    [Required]
    public Reader Reader { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime Expires { get; set; } = DateTime.Now;
}
