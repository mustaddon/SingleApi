using System.IO;

namespace SingleApi
{
    public interface ISapiFile
    {
        Stream Content { get; set; }
        string? Type { get; set; }
        string? Name { get; set; }
        long Size { get; set; }
    }

    public interface ISapiFileResult : ISapiFile
    {
        bool Inline { get; set; }
    }
}
