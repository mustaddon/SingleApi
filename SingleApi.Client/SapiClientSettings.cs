using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SingleApi.Client
{
    public class SapiClientSettings
    {
        public ICredentials? Credentials { get; set; }

        public Dictionary<string, IEnumerable<string>> DefaultRequestHeaders { get; set; } = new();

        public JsonSerializerOptions JsonSerializerOptions { get; set; } = new()
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
        };
    }
}
