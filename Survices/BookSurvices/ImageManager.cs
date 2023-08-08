using LibraryV2.Models;
using LibraryV2.Repository.Interfaces;

namespace LibraryV2.Survices.BookSurvices;

public class ImageManager : IImageManager
{
    private readonly IBookCoverRepository _repository;

    public ImageManager(IBookCoverRepository repository)
    {
        _repository = repository;
    }

    public async Task<BookCover> PostFileAsync(IFormFile fileData, Book book)
    {
        if (fileData is null)
        {
            return null;
        }

        try
        {
            var bookCover = new BookCover()
            {
                ImageName = DateTime.UtcNow.Ticks.ToString() + fileData.FileName
            };

            using (var stream = new MemoryStream())
            {
                fileData.CopyTo(stream);
                bookCover.ImageData = stream.ToArray();
            }

            return await _repository.CreateBookCover(bookCover);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<byte[]> DownloadFileById(Ulid id)
    {
        try
        {
            var file = await _repository.GetBookCover(id);

            return file.ImageData;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
