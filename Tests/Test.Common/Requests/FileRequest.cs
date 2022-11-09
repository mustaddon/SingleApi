using MediatR;
using MetaFile;

namespace Test.Requests
{
    public class FileRequest : IRequest<HttpFile<FileMetadata>>
    {
        public string Name { get; set; } = string.Empty;
    }
}
