using SingleApi.Client;

namespace Test.MediatR
{
    public class Tests : IDisposable
    {
        readonly SapiClient _client = new ($"{Settings.WebApiUrl}mediatr");

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