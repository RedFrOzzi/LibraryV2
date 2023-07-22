using LibraryV2.Dto;
using LibraryV2.Models;
using Riok.Mapperly.Abstractions;

namespace LibraryV2.Mapper;

[Mapper]
public partial class BookEditionMapper
{
    public partial BookEditionDto EditionToEditionDto(BookEdition edition);
    public partial BookEdition EditionDtoToEdition(BookEditionDto editionDto);
    public partial IReadOnlyList<BookEditionDto> EditionToEditionDto(IReadOnlyList<BookEdition> edition);
    public partial IReadOnlyList<BookEdition> EditionDtoToEdition(IReadOnlyList<BookEditionDto> editionDtos);
}
