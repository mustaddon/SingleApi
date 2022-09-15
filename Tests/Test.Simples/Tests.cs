using SingleApi.Client;

namespace Test.Simples
{
    public class Tests : IDisposable
    {
        readonly SapiClient _client = new ($"{Settings.WebApiUrl}sapi");

        public void Dispose()
        {
            _client?.Dispose();
            GC.SuppressFinalize(this);
        }


        [Test]
        public async Task TestString()
        {
            var request = "test";

            var response = await _client.Send<string, string>(request);

            Assert.That(response, Is.EqualTo(request));
        }

        [Test]
        public async Task TestInt()
        {
            var request = 555;

            var response = await _client.Send<int, int>(request);

            Assert.That(response, Is.EqualTo(request));
        }

        [Test]
        public async Task TestDateTime()
        {
            var request = DateTime.Now;

            var response = await _client.Send<DateTime, DateTime>(request);

            Assert.That(response, Is.EqualTo(request));
        }
    }
}