using MediatR;
namespace Example.MediatR;

public class FileUploadHandler : IRequestHandler<FileUpload, string>
{
    public async Task<string> Handle(FileUpload request, CancellationToken cancellationToken)
    {
        var filePath = Path.GetFullPath(request.Name ?? throw new ArgumentNullException(nameof(request.Name)));
        using var fileStream = File.Create(filePath);
        await request.Content.CopyToAsync(fileStream, cancellationToken);
        return filePath;
    }
}

