using MediatR;
using SingleApi;
using System.IO;

namespace Example;

public class FileUpload : IRequest<string>, ISapiFile
{
    public Stream Content { get; set; } = Stream.Null;
    public string? Type { get; set; }
    public string? Name { get; set; }
}
