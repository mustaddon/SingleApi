using SingleApi.Client;

namespace Test
{
    public class Settings
    {
        public static readonly string WebApiUrl = "https://localhost:7263/";
        public static readonly string TempPath = @".\_tmp\";

        public static readonly SapiClientSettings Client = new()
        {
            DefaultRequestHeaders = new() {
                { "sapi-test", new [] { "test_value" } },
            }
        };
    }
}
