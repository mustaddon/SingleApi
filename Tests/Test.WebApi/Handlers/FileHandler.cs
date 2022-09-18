using MediatR;
using SingleApi;
using System.Text;

namespace Test.WebApi.Handlers
{
    public class FileHandler : IRequestHandler<FileRequest, SapiFile<FileMetadata>>
    {
        public async Task<SapiFile<FileMetadata>> Handle(FileRequest request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            var metadata = new FileMetadata
            {
                Author = "Test",
                Date = DateTime.Now,
            };

            if (string.IsNullOrEmpty(request.Name))
                return new ()
                {
                    Content = new MemoryStream(Encoding.UTF8.GetBytes(TestData.FileContent)),
                    Name = TestData.FileName,
                    Type = TestData.FileType,
                    Metadata = metadata,
                };

            var path = Path.GetFullPath(Path.Combine(@".\files\", request.Name));

            if (!File.Exists(path))
                throw new FileNotFoundException(path);

            return new()
            {
                Content = File.OpenRead(path),
                Name = request.Name,
                Type = typeMap.TryGetValue(Path.GetExtension(request.Name).ToLower(), out var type) ? type : null,
                Disposition = SapiFileDispositions.Inline,
                Metadata = metadata,
            };
        }

        static readonly Dictionary<string, string> typeMap = new()
        {
            { ".html", "text/html" },
            { ".txt", "text/plain" },
        };
    }
}
