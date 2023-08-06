using LibraryV2.Models;
using Microsoft.VisualBasic.FileIO;

namespace LibraryV2.Survices.BookSurvices;

public interface IImageManager
{
    Task<BookCover> PostFileAsync(IFormFile fileData);

    Task<List<BookCover>> PostMultiFileAsync(List<IFormFile> filesData);

    Task<byte[]> DownloadFileById(Ulid id);
}
