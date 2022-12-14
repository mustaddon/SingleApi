namespace Test.Client
{
    public partial class Sapi : IDisposable
    {
        readonly SapiClient _client = new($"{Settings.WebApiUrl}sapi");

        public void Dispose()
        {
            _client?.Dispose();
            GC.SuppressFinalize(this);
        }


        [Test]
        public async Task TestString()
        {
            var request = "test";
            var response = await _client.Send<string>(request);

            Assert.That(response, Is.EqualTo(request));
        }

        [Test]
        public async Task TestInt()
        {
            var request = 555;
            var response = await _client.Send<int>(request);

            Assert.That(response, Is.EqualTo(request));
        }

        [Test]
        public async Task TestNullable()
        {
            var request = (long?)777;
            var response = await _client.Send<long?>(request);
            var responseNull = await _client.Send<long?, long?>(null);

            Assert.Multiple(() =>
            {
                Assert.That(response, Is.EqualTo(request));
                Assert.That(responseNull, Is.Null);
            });
        }

        [Test]
        public async Task TestDateTime()
        {
            var request = DateTime.Now;
            var response = await _client.Send<DateTime>(request);

            Assert.That(response, Is.EqualTo(request));
        }
    }
}