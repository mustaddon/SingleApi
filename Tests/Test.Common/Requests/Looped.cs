using MediatR;

namespace Test.Requests
{
    public class Looped : IRequest<Looped>
    {
        public Looped? Child { get; set; }
        public int Number { get; set; }

        public readonly int ReadonlyField = 111;
        public int ReadonlyProp => 222;
    }
}
