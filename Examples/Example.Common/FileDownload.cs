using MediatR;
using MetaFile;

namespace Example;

public class FileDownload : IRequest<HttpFile>
{
    public string? Path { get; set; }
}
