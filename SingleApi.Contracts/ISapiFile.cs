using System.IO;

namespace SingleApi
{
    public interface ISapiFile
    {
        Stream Content { get; set; }
        string? Type { get; set; }
        string? Name { get; set; }
    }

    public interface ISapiFile<TMetadata> : ISapiFile
    {
        TMetadata? Metadata { get; set; }
    }
}
