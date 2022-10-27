using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using TypeSerialization;

namespace SingleApi
{
    class SapiEndpoint
    {
        public SapiEndpoint(IEnumerable<Type> types, JsonSerializerOptions? jsonOptions = null)
        {
            _typeDeserializer = new TypeDeserializer(types
                .Concat(typeof(SapiFile).Assembly.GetTypes())
                .Where(x => !x.IsAbstract && !x.IsAssignableTo(typeof(Attribute))));

            _jsonOptions = jsonOptions ?? DefaultJsonOptions;
        }

        readonly JsonSerializerOptions _jsonOptions;
        readonly TypeDeserializer _typeDeserializer;

        public Task<IResult> ProcessGet(HttpContext ctx, string typeStr, string? data, SapiDelegate handler, CancellationToken cancellationToken)
        {
            var type = _typeDeserializer.Deserialize(typeStr);

            ctx.Response.Headers.AddNoCache();

            return GetResult(ctx, handler(new SapiDelegateArgs(
                httpContext: ctx,
                dataType: type,
                data: data == null ? null : JsonSerializer.Deserialize(data, type, _jsonOptions),
                cancellationToken: cancellationToken)));
        }

        public async Task<IResult> ProcessPost(HttpContext ctx, string typeStr, SapiDelegate handler, CancellationToken cancellationToken)
        {
            var type = _typeDeserializer.Deserialize(typeStr);

            return await GetResult(ctx, handler(new SapiDelegateArgs(
                httpContext: ctx,
                dataType: type,
                data: await GetData(ctx, type, cancellationToken),
                cancellationToken: cancellationToken)));
        }

        async Task<object?> GetData(HttpContext ctx, Type type, CancellationToken cancellationToken)
        {
            if (typeof(ISapiFile).IsAssignableFrom(type))
                return ctx.Request.ToSapiFile(type, _jsonOptions);

            return await JsonSerializer.DeserializeAsync(ctx.Request.Body, type, _jsonOptions, cancellationToken);
        }

        async Task<IResult> GetResult(HttpContext ctx, Task<object?> valueTask)
        {
            var value = await valueTask;

            if (value is IResult result)
                return result;

            if (value is ISapiFileReadOnly file)
                return file.ToResult(ctx.Response, _jsonOptions);

            if (value is Stream stream)
                return Results.Stream(stream);

            return Results.Json(value, _jsonOptions);
        }

        static readonly JsonSerializerOptions DefaultJsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
        };
    }
}
