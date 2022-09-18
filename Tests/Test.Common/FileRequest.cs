using MediatR;
using SingleApi;

namespace Test
{
    public class FileRequest : IRequest<SapiFile<FileMetadata>>
    {
        public string Name { get; set; } = string.Empty;
    }
}
