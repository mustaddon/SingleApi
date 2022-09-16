using SingleApi.Client;
using System.Text;

namespace Test.Files
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
        public async Task TestDownload()
        {
            using var response = await _client.Send(new FileRequest());

            Assert.That(response?.Name, Is.EqualTo(TestData.FileName));
            Assert.That(response?.Type, Is.EqualTo(TestData.FileType));
            Assert.That(response?.Content.ToText(), Is.EqualTo(TestData.FileContent));
        }

    }
}