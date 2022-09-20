using System.IO;

namespace SingleApi
{
    public interface ISapiFile : ISapiFileReadOnly
    {
        new Stream Content { get; set; }
        new string? Type { get; set; }
        new string? Name { get; set; }
    }

    public interface ISapiFile<TMetadata> : ISapiFile, ISapiFileReadOnly<TMetadata>
    {
        new TMetadata? Metadata { get; set; }
    }
}
