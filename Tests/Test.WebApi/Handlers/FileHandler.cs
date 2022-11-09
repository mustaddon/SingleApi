using MediatR;
using MetaFile;
using SingleApi;
using System.Text;
using Test.Requests;

namespace Test.WebApi.Handlers
{
    public class FileHandler : IRequestHandler<FileRequest, HttpFile<FileMetadata>>
    {
        public async Task<HttpFile<FileMetadata>> Handle(FileRequest request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            if (string.IsNullOrEmpty(request.Name))
                return new ()
                {
                    Content = new MemoryStream(Encoding.UTF8.GetBytes(TestData.FileContent)),
                    Name = TestData.FileName,
                    Type = TestData.FileType,
                    Metadata = TestData.FileMetadata,
                };

            var path = Path.GetFullPath(Path.Combine(@".\files\", request.Name));

            return new()
            {
                Content = File.OpenRead(path),
                Name = request.Name,
                Type = typeMap.TryGetValue(Path.GetExtension(request.Name).ToLower(), out var type) ? type : null,
                InlineDisposition = true,
            };
        }

        static readonly Dictionary<string, string> typeMap = new()
        {
            { ".html", "text/html" },
            { ".txt", "text/plain" },
        };
    }
}
