namespace Test.Client
{
    public partial class MediatR
    {
        [Test]
        public async Task TestStreamDownload()
        {
            using var response = await _client.Send(new StreamRequest());
            Assert.That(response?.ToText(), Is.EqualTo(TestData.FileContent));
        }

        [Test]
        public async Task TestFileDownload()
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
        public async Task TestFileUpload()
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

#if NET6_0_OR_GREATER
            var responseContent = response?.Path == null ? Array.Empty<byte>() : await File.ReadAllBytesAsync(response.Path);
#else
            var responseContent = response == null ? Array.Empty<byte>() : File.ReadAllBytes(response.Path);
#endif

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
