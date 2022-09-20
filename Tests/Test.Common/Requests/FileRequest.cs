using MediatR;
using SingleApi;

namespace Test.Requests
{
    public class FileRequest : IRequest<SapiFile<FileMetadata>>
    {
        public string Name { get; set; } = string.Empty;
    }
}
