using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using TypeSerialization;

namespace SingleApi.Client;

public class SapiClientSettings
{
    internal static readonly SapiClientSettings Default = new();


    public TypeDeserializer TypeDeserializer { get; set; } = TypeDeserializers.Default;

    public ICredentials? Credentials { get; set; }

    public Dictionary<string, IEnumerable<string>> DefaultRequestHeaders { get; set; } = new();

    public JsonSerializerOptions JsonSerializerOptions { get; set; } = new()
    {
        PropertyNameCaseInsensitive = true,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
    };
}
