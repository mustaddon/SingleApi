using SingleApi.Client;
using System.Text;

namespace Test.Files
{
    public class Tests : IDisposable
    {
        readonly SapiClient _client = new ($"{Settings.WebApiUrl}mediatr", Settings.Client);

        public void Dispose()
        {
            _client?.Dispose();
            GC.SuppressFinalize(this);
        }

        [Test]
        public async Task TestDownload()
        {
            using var response = await _client.Send(new FileRequest());
            
            Assert.Multiple(() =>
            {
                Assert.That(response?.Name, Is.EqualTo(TestData.FileName));
                Assert.That(response?.Type, Is.EqualTo(TestData.FileType));
                Assert.That(response?.Content?.ToText(), Is.EqualTo(TestData.FileContent));
                Assert.That(response?.Metadata?.User, Is.EqualTo(TestData.FileMetadata.User));
                Assert.That(response?.Metadata?.Date, Is.EqualTo(TestData.FileMetadata.Date));
            });
        }


        [Test]
        public async Task TestUpload()
        {
            var requestContent = Encoding.UTF8.GetBytes(TestData.FileContent);

            var request = new FileUpload<FileMetadata>
            {
                Content = new MemoryStream(requestContent),
                Name = TestData.FileName,
                Type = TestData.FileType,
                Metadata = TestData.FileMetadata,
            };

            var response = await _client.Send(request);

            Assert.That(response?.Path, Is.Not.Null);

            var responseContent = await File.ReadAllBytesAsync(response.Path);

            Assert.Multiple(() =>
            {
                Assert.That(responseContent, Is.EquivalentTo(requestContent));
                Assert.That(response?.Name, Is.EqualTo(request.Name));
                Assert.That(response?.Type, Is.EqualTo(request.Type));
                Assert.That(response?.Metadata?.User, Is.EqualTo(request.Metadata?.User));
                Assert.That(response?.Metadata?.Date, Is.EqualTo(request.Metadata?.Date));
            });
        }
    }
}