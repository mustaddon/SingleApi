using MediatR;

namespace Test.WebApi.Handlers
{
    public class FileUploadHandler : IRequestHandler<FileUpload, Guid>
    {
        public async Task<Guid> Handle(FileUpload request, CancellationToken cancellationToken)
        {
            var path = Path.GetFullPath(@$".\files\upload.tmp");
            using var file = File.Create(path);
            await request.Content.CopyToAsync(file, cancellationToken);
            return Guid.NewGuid();
        }
    }
}
