using LibraryV2.Dto.PostDto;
using LibraryV2.Models;
using LibraryV2.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LibraryV2.Mapper
{
    public interface IPostModelMapper
    {
        Task<Book> BookPostDtoToBook(BookPostDto bookPostDto,
                                     ModelStateDictionary modelState,
                                     IBookEditionRepository bookEditionRepository,
                                     IAuthorRepository authorRepository);

        Task<BookEdition> BookEditionPostDtoToBook(BookEditionPostDto bookEditionPostDto,
                                                   ModelStateDictionary modelState,
                                                   IBookRepository bookRepository);

        Task<Author> AuthorPostDtoToBook(AuthorPostDto authorPostDto,
                                         ModelStateDictionary modelState,
                                         IBookRepository bookRepository);

        Reader ReaderPostDtoToReader(ReaderPostDto readerPostDto, ModelStateDictionary modelState);
    }
}
