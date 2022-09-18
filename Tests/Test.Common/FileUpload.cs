using MediatR;
using SingleApi;
using System;
using System.IO;

namespace Test
{
    public class FileUpload : ISapiFile<FileMetadata>, IRequest<Guid>
    {
        public Stream Content { get; set; } = Stream.Null;
        public string? Type { get; set; }
        public string? Name { get; set; }
        public FileMetadata? Metadata { get; set; }
    }
}
