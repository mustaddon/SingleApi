using MediatR;
using SingleApi;

namespace Example;

public class FileDownload : IRequest<SapiFile>
{
    public string? Path { get; set; }
}
