using MediatR;
using Test.Requests;

namespace Test.WebApi.Handlers
{
    public class FileUploadHandler : IRequestHandler<FileUpload, FileUploadResult>
    {
        public async Task<FileUploadResult> Handle(FileUpload request, CancellationToken cancellationToken)
        {
            var path = Path.GetFullPath(@$".\files\upload.tmp");
            using var file = File.Create(path);
            await request.Content.CopyToAsync(file, cancellationToken);

            return new FileUploadResult
            {
                Path = path,
                Name = request.Name,
                Type = request.Type,
            };
        }
    }

    public class FileUploadHandler<TMetadata> : IRequestHandler<FileUpload<TMetadata>, FileUploadResult<TMetadata>>
    {
        public async Task<FileUploadResult<TMetadata>> Handle(FileUpload<TMetadata> request, CancellationToken cancellationToken)
        {
            var path = Path.GetFullPath(@$".\files\upload.tmp");
            using var file = File.Create(path);
            await request.Content.CopyToAsync(file, cancellationToken);
            
            return new FileUploadResult<TMetadata>
            {
                Path = path,
                Name = request.Name,
                Type = request.Type,
                Metadata = request.Metadata,
            };
        }
    }
}
