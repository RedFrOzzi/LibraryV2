using LibraryV2.Dto;
using LibraryV2.Models;
using Riok.Mapperly.Abstractions;

namespace LibraryV2.Mapper;

[Mapper]
public partial class AuthorMapper
{

    public partial AuthorDto AuthorToAuthorDto(Author author);
    public partial Author AuthorDtoToAuthor(AuthorDto authorDto);
    public partial IReadOnlyList<AuthorDto> AuthorToAuthorDto(IReadOnlyList<Author> authors);
    public partial IReadOnlyList<Author> AuthorDtoToAuthor(IReadOnlyList<AuthorDto> authorDtos);
}
