using System;
using System.IO;

namespace SingleApi
{
    public class SapiFile : ISapiFile, ISapiFileResponse, IDisposable
    {
        public Stream Content { get; set; } = Stream.Null;
        public string? Type { get; set; }
        public string? Name { get; set; }
        public bool InlineDisposition { get; set; }

        public void Dispose()
        {
            Content.Dispose();
            GC.SuppressFinalize(this);
        }
    }

    public class SapiFile<TMetadata> : SapiFile, ISapiFile<TMetadata>
    {
        public TMetadata? Metadata { get; set; }
    }
}
