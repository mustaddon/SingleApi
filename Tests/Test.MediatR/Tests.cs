using SingleApi.Client;

namespace Test.MediatR
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            _client = new SapiClient($"{Settings.WebApiUrl}mediatr");
        }

        SapiClient _client;

        [Test]
        public async Task TestPingPong()
        {
            var response = await _client.Send(new Ping { Message = "TEST" });

            Assert.That(response?.Message, Is.EqualTo("TEST PONG"));
        }

    }
}