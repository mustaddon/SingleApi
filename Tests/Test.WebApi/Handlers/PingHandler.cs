using MediatR;

namespace Test.WebApi.Handlers
{
    public class PingHandler : IRequestHandler<Ping, Pong>
    {
        public Task<Pong> Handle(Ping request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Pong { Message = request.Message + " PONG" });
        }
    }
}
