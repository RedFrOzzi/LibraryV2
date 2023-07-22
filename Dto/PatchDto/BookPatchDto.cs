namespace LibraryV2.Dto.PatchDto;

public class BookPatchDto
{
    public string Id { get; set; }
    public string? Title { get; set; }
    public string[]? AuthorIds { get; set; }
    public string? EditionId { get; set; }
    public DateTime? ReleaseDate { get; set; }
}
