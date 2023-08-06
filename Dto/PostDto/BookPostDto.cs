namespace LibraryV2.Dto.PostDto;

public class BookPostDto
{
    public string Title { get; set; }
    public IFormFile? BookCover { get; set; }
    public string[]? AuthorIds { get; set; }
    public string? EditionId { get; set; }
    public DateTime? ReleaseDate { get; set; }
}
