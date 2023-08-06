using LibraryV2.Dto;
using LibraryV2.Dto.PostDto;
using LibraryV2.Models;
using LibraryV2.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Riok.Mapperly.Abstractions;

namespace LibraryV2.Mapper;

[Mapper]
public partial class BookMapper
{
    public partial BookDto BookToBookDto(Book book);
    public partial Book BookDtoToBook(BookDto bookDto);
    public partial IList<BookDto> BookToBookDto(IReadOnlyList<Book> books);
    public partial IList<Book> BookDtoToBook(IReadOnlyList<BookDto> bookDtos);
}
