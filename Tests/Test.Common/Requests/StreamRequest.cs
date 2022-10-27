using MediatR;
using System.IO;

namespace Test.Requests
{
    public class StreamRequest : IRequest<Stream>
    {
        public string Name { get; set; } = string.Empty;
    }
}
