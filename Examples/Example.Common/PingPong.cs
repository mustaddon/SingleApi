using MediatR;

namespace Example;

public class Ping : IRequest<Pong>
{
    public string? Message { get; set; }
}

public class Pong
{
    public string? Message { get; set; }
}
