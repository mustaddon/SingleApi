using SingleApi.Client;

namespace Test.Generics
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            _client = new SapiClient($"{Settings.WebApiUrl}sapi");
        }

        SapiClient _client;

        [Test]
        public async Task TestArray()
        {
            var data = new int?[] { 555, null, 666 };

            var response = await _client.Send<int?[], int?[]>(data);

            Assert.That(response, Is.EquivalentTo(data));
        }

        [Test]
        public async Task TestList()
        {
            var data = new List<int> { 111, 222, 333 };

            var response = await _client.Send<List<int>, List<int>>(data);

            Assert.That(response, Is.EquivalentTo(data));
        }

        [Test]
        public async Task TestDictionary()
        {
            var data = new Dictionary<string, int[]> {
                { "key_1", new [] { 1, 11, 111 } },
                { "key_2", new [] { 2, 22, 222 } },
            };

            var response = await _client.Send<Dictionary<string, int[]>, Dictionary<string, int[]>>(data);

            Assert.That(response, Is.EquivalentTo(data));
        }
    }
}