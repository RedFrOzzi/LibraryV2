using LibraryV2.Dto.PatchDto;
using LibraryV2.Dto.PostDto;
using LibraryV2.Models;
using LibraryV2.Repository.Interfaces;
using LibraryV2.Survices.BookSurvices;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LibraryV2.Survices;

public interface IModelMapperService
{
    Task<Book> BookPostDtoToBook(BookPostDto bookPostDto,
                                 ModelStateDictionary modelState,
                                 IBookEditionRepository bookEditionRepository,
                                 IAuthorRepository authorRepository);

    Task<Book> BookPatchDtoToBook(Book book,
                                  BookPatchDto bookPatchDto,
                                  ModelStateDictionary modelState,
                                  IImageManager imageManager,
                                  IBookEditionRepository bookEditionRepository,
                                  IAuthorRepository authorRepository,
                                  IBookCoverRepository bookCoverRepository);

    Task<BookEdition> BookEditionPostDtoToBookEdition(BookEditionPostDto bookEditionPostDto,
                                                      ModelStateDictionary modelState,
                                                      IBookRepository bookRepository);

    Task<Author> AuthorPostDtoToAuthor(AuthorPostDto authorPostDto,
                                       ModelStateDictionary modelState,
                                       IBookRepository bookRepository);

    Reader ReaderPostDtoToReader(ReaderPostDto readerPostDto,
                                 ModelStateDictionary modelState);
}
