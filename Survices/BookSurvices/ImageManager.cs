using LibraryV2.DataContext;
using LibraryV2.Models;
using LibraryV2.Repository.Interfaces;
using LibraryV2.Utilities;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.VisualBasic.FileIO;

namespace LibraryV2.Survices.BookSurvices
{
    public class ImageManager : IImageManager
    {
        private readonly IBookCoverRepository _repository;

        public ImageManager(IBookCoverRepository repository)
        {
            _repository = repository;
        }

        public async Task<BookCover> PostFileAsync(IFormFile fileData)
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

        public async Task<List<BookCover>> PostMultiFileAsync(List<IFormFile> filesData)
        {
            if (filesData is null || filesData.Count == 0)
            {
                return null;
            }

            try
            {
                List<BookCover> bookCovers = new();

                foreach (var fileData in filesData)
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

                    bookCovers.Add(await _repository.CreateBookCover(bookCover));
                }

                return bookCovers;
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

                //var content = new MemoryStream(file.ImageData);
                //var path = Path.Combine(
                //   Directory.GetCurrentDirectory(), "FileDownloaded",
                //   file.ImageName);

                //await CopyStream(content, path);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task CopyStream(Stream stream, string downloadPath)
        {
            using (var fileStream = new FileStream(downloadPath, FileMode.Create, FileAccess.Write))
            {
                await stream.CopyToAsync(fileStream);
            }
        }
    }
}
