using LibraryV2.Models;

namespace LibraryV2.Survices.BookSurvices;

public interface IImageManager
{
    Task<BookCover> PostFileAsync(IFormFile fileData, Book book);

    Task<byte[]> DownloadFileById(Ulid id);
}
