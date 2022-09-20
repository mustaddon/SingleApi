using MediatR;
using SingleApi;
using System.IO;

namespace Test.Requests
{
    public class FileUpload : ISapiFile, IRequest<FileUploadResult>
    {
        public Stream Content { get; set; } = Stream.Null;
        public string? Type { get; set; }
        public string? Name { get; set; }
    }

    public class FileUpload<TMetadata> : ISapiFile<TMetadata>, IRequest<FileUploadResult<TMetadata>>
    {
        public Stream Content { get; set; } = Stream.Null;
        public string? Type { get; set; }
        public string? Name { get; set; }
        public TMetadata? Metadata { get; set; }
    }

    public class FileUploadResult
    {
        public string? Type { get; set; }
        public string? Name { get; set; }
        public string? Path { get; set; }
    }

    public class FileUploadResult<TMetadata> : FileUploadResult
    {
        public TMetadata? Metadata { get; set; }
    }
}
