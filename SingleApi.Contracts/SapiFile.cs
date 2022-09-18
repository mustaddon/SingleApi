using System;
using System.IO;

namespace SingleApi
{
    public class SapiFile : ISapiFile, ISapiFileResponse, IDisposable
    {
        public Stream Content { get; set; } = Stream.Null;
        public string? Type { get; set; }
        public string? Name { get; set; }
        public SapiFileDispositions Disposition { get; set; } = SapiFileDispositions.Attachment;

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
