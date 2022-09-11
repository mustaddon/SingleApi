using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace SingleApi
{
    class SapiEndpoint
    {
        public SapiEndpoint(IEnumerable<Type> types)
        {
            _typeDeserializer = new TypeDeserializer(types.Where(x => !x.IsAbstract));
        }

        readonly TypeDeserializer _typeDeserializer;

        public Task<object?> ProcessGet(HttpContext ctx, string typeStr, string data, SapiDelegate handler, CancellationToken cancellationToken)
        {
            var type = _typeDeserializer.Deserialize(typeStr);

            ctx.Response.Headers.AddNoCache();

            return handler(new SapiDelegateArgs(
                httpContext: ctx,
                dataType: type,
                data: JsonSerializer.Deserialize(data, type, JsonSerializerOptions),
                cancellationToken: cancellationToken));
        }

        public Task<object?> ProcessPost(HttpContext ctx, string typeStr, JsonElement json, SapiDelegate handler, CancellationToken cancellationToken)
        {
            var type = _typeDeserializer.Deserialize(typeStr);

            return handler(new SapiDelegateArgs(
                httpContext: ctx,
                dataType: type,
                data: json.Deserialize(type, JsonSerializerOptions),
                cancellationToken: cancellationToken));
        }

        static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
        };
    }
}
