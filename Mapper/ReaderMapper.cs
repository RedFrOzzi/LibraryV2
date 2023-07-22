using LibraryV2.Dto;
using LibraryV2.Models;
using Microsoft.EntityFrameworkCore.Internal;
using Riok.Mapperly.Abstractions;

namespace LibraryV2.Mapper
{
    [Mapper]
    public partial class ReaderMapper
    {
        public partial ReaderDto ReaderToReaderDto(Reader reader);
        public partial Reader ReaderDtoToReader(ReaderDto readerDto);
        public partial IReadOnlyList<ReaderDto> ReaderToReaderDto(IReadOnlyList<Reader> readers);
        public partial IReadOnlyList<Reader> ReaderDtoToReader(IReadOnlyList<ReaderDto> readerDtos);
    }
}
