using MediatR;
using MetaFile;

namespace Test.Requests
{
    public class FileUpload : StreamFile, IRequest<FileUploadResult>
    {
    }

    public class FileUpload<TMetadata> : StreamFile<TMetadata>, IRequest<FileUploadResult<TMetadata>>
    {

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
