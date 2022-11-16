using System.Text.Json;
using System.Text.Json.Serialization;

namespace SingleApi;

public sealed class SapiOptions
{
    public JsonSerializerOptions JsonSerialization { get; set; } = new()
    {
        PropertyNameCaseInsensitive = true,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
    };
}
