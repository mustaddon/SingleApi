using MediatR;

namespace Test.Requests
{
    public class Looped : IRequest<Looped>
    {
        public Looped? Child { get; set; }
        public int Number { get; set; }
    }
}
