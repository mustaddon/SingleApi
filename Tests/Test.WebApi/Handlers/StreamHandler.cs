using MediatR;
using System.Text;
using Test.Requests;

namespace Test.WebApi.Handlers
{
    public class StreamHandler : IRequestHandler<StreamRequest, Stream>
    {
        public async Task<Stream> Handle(StreamRequest request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            if (string.IsNullOrEmpty(request.Name))
                return new MemoryStream(Encoding.UTF8.GetBytes(TestData.FileContent));

            var path = Path.GetFullPath(Path.Combine(@".\files\", request.Name));

            return File.OpenRead(path);
        }
    }
}
