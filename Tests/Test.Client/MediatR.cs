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
    }
}
