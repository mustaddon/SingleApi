using MediatR;
using MetaFile;

namespace Example;

public class FileUpload : StreamFile, IRequest<string>
{

}
