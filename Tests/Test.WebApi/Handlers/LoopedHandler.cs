using MediatR;
using Test.Requests;

namespace Test.WebApi.Handlers
{
    public class LoopedHandler : IRequestHandler<Looped, Looped>
    {
        public Task<Looped> Handle(Looped request, CancellationToken cancellationToken)
        {
            request.Child = request;
            return Task.FromResult(request);
        }
    }
}
