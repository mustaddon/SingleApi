using MediatR;
namespace Example.MediatR;

public class PingHandler : IRequestHandler<Ping, Pong>
{
    public Task<Pong> Handle(Ping request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Pong { Message = request.Message + " PONG" });
    }
}

public class Ping : IRequest<Pong>
{
    public string Message { get; set; }
}

public class Pong
{
    public string Message { get; set; }
}
