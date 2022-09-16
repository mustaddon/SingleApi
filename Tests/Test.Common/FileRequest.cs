using MediatR;
using SingleApi;

namespace Test
{
    public class FileRequest : IRequest<SapiFile>
    {
        public string Name { get; set; }
    }
}
