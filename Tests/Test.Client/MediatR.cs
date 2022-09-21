namespace Test.Client
{
    public partial class MediatR
    {
        readonly SapiClient _client = new($"{Settings.WebApiUrl}mediatr", Settings.Client);

        public void Dispose()
        {
            _client?.Dispose();
            GC.SuppressFinalize(this);
        }

        [Test]
        public async Task TestPingPong()
        {
            var response = await _client.Send(new Ping { Message = "TEST" });

            Assert.That(response?.Message, Is.EqualTo("TEST PONG"));
        }

        [Test]
        public async Task TestLoop()
        {
            var request = new Looped { Number = 666 };
            request.Child = request;
            var response = await _client.Send(request);

            Assert.That(response?.Child, Is.Null);
            Assert.That(response?.Number, Is.EqualTo(request.Number));
            Assert.That(response?.ReadonlyField, Is.EqualTo(request.ReadonlyField));
            Assert.That(response?.ReadonlyProp, Is.EqualTo(request.ReadonlyProp));
        }
    }
}
