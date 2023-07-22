namespace LibraryV2.Dto.PatchDto;

public class AuthorPatchDto
{
    public string Id { get; set; }
    public string? Name { get; set; }
    public string[]? BookIds { get; set; }
}
