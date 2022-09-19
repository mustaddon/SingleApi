using MediatR;
using SingleApi;
namespace Example.MediatR;

public class FileDownloadHandler : IRequestHandler<FileDownload, SapiFile>
{
    public Task<SapiFile> Handle(FileDownload request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new SapiFile()
        {
            Content = File.OpenRead(Path.GetFullPath(request.Path)),
            Name = Path.GetFileName(request.Path),
        });
    }
}

