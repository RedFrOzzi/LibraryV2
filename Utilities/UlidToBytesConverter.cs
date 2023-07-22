using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LibraryV2.Utilities
{
    public class UlidToBytesConverter : ValueConverter<Ulid, byte[]>
    {
        public UlidToBytesConverter()
            : base(
                    convertToProviderExpression: x => x.ToByteArray(),
                    convertFromProviderExpression: x => new Ulid(x))
        {
        }
    }
}
