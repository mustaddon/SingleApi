namespace Test.Client
{
    public partial class Sapi
    {
        [Test]
        public async Task TestStream()
        {
            using var request = new MemoryStream(Encoding.UTF8.GetBytes(TestData.FileContent));
            using var response = await _client.Send<Stream>(request);
            Assert.That(response?.ToText(), Is.EqualTo(TestData.FileContent));
        }
    }
}
