using System.IO;

namespace SingleApi
{
    public interface ISapiFileReadOnly
    {
        Stream Content { get; }
        string? Type { get; }
        string? Name { get; }
    }

    public interface ISapiFileReadOnly<out TMetadata> : ISapiFileReadOnly
    {
        TMetadata? Metadata { get; }
    }
}
