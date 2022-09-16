using MediatR;
using SingleApi;
using System.Text;

namespace Test.WebApi.Handlers
{
    public class FileHandler : IRequestHandler<FileRequest, SapiFile>
    {
        public Task<SapiFile> Handle(FileRequest request, CancellationToken cancellationToken)
        {
            if (request.Name == null)
                return Task.FromResult(new SapiFile(new MemoryStream(Encoding.UTF8.GetBytes(TestData.FileContent)))
                {
                    Name = TestData.FileName,
                    Type = TestData.FileType,
                    Inline = true,
                });

            var path = Path.GetFullPath(Path.Combine(@".\files\", request.Name));

            if (!File.Exists(path))
                throw new FileNotFoundException(path);

            return Task.FromResult(new SapiFile(File.OpenRead(path))
            {
                Name = request.Name,
                Type = typeMap.TryGetValue(Path.GetExtension(request.Name).ToLower(), out var type) ? type : null,
                Inline = true
            });
        }

        static readonly Dictionary<string, string> typeMap = new()
        {
            { ".html", "text/html" },
            { ".txt", "text/plain" },
        };
    }
}
