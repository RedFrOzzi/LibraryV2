using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryV2.Models
{
    public class Author
    {
        public Author()
        {
            Id = Ulid.NewUlid();
        }
        [Key]
        public Ulid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Book>? Books { get; set; } = new List<Book>();
    }
}
