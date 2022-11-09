using MediatR;
using MetaFile;
namespace Example.MediatR;

public class FileDownloadHandler : IRequestHandler<FileDownload, HttpFile>
{
    public Task<HttpFile> Handle(FileDownload request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new HttpFile()
        {
            Content = File.OpenRead(Path.GetFullPath(request.Path!)),
            Name = Path.GetFileName(request.Path),
        });
    }
}

