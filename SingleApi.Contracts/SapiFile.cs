using System;
using System.IO;

namespace SingleApi
{
    public class SapiFile : ISapiFileResult, IDisposable
    {
        public SapiFile(Stream content)
        {
            Content = content;
        }

        public Stream Content { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public bool Inline { get; set; }
        public long Size { get; set; }

        public void Dispose()
        {
            Content?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
