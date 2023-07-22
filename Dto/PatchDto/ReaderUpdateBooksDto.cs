namespace LibraryV2.Dto.PatchDto
{
    public class ReaderUpdateBooksDto
    {
        public string Id { get; set; }
        public string[]? BorrowedBookIds { get; set; }
    }
}
