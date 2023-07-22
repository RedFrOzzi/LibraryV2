using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryV2.Models
{
    public class BookEdition
    {
        public BookEdition()
        {
            Id= Ulid.NewUlid();
        }
        [Key]
        public Ulid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Book>? Books { get; set; } = new List<Book>();
    }
}
