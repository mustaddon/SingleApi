using MetaFile;
using MetaFile.Http.AspNetCore;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using TypeSerialization;

namespace SingleApi;

class SapiEndpoint
{
    public SapiEndpoint(SapiDelegate handler, IEnumerable<Type> types, SapiOptions options)
    {
        _handler = handler;
        _typeDeserializer = TypeDeserializers.Create(types);
        _options = options;
    }

    readonly SapiDelegate _handler;
    readonly TypeDeserializer _typeDeserializer;
    readonly SapiOptions _options;

    public Task<IResult> ProcessGet(HttpContext ctx, string type, string? data)
    {
        var typeObj = _typeDeserializer.Deserialize(type)!;

        ctx.Response.Headers.AddNoCache();

        return GetResult(ctx, _handler(new SapiDelegateArgs(
            httpContext: ctx,
            dataType: typeObj,
            data: data == null ? null : JsonSerializer.Deserialize(data, typeObj, _options.JsonSerialization),
            cancellationToken: ctx.RequestAborted)));
    }

    public async Task<IResult> ProcessPost(HttpContext ctx, string type)
    {
        var typeObj = _typeDeserializer.Deserialize(type)!;

        return await GetResult(ctx, _handler(new SapiDelegateArgs(
            httpContext: ctx,
            dataType: typeObj,
            data: await GetData(ctx, typeObj, ctx.RequestAborted),
            cancellationToken: ctx.RequestAborted)));
    }

    async Task<object?> GetData(HttpContext ctx, Type type, CancellationToken cancellationToken)
    {
        if (type == Types.Stream)
            return new SapiStream(ctx.Request);

        if (Types.IStreamFile.IsAssignableFrom(type))
            return ctx.Request.ToStreamFile(type, _options.JsonSerialization);

        return await JsonSerializer.DeserializeAsync(ctx.Request.Body, type, _options.JsonSerialization, cancellationToken);
    }

    async Task<IResult> GetResult(HttpContext ctx, Task<object?> valueTask)
    {
        var value = await valueTask;

        if (value == null)
            return Results.NoContent();

        ctx.Response.Headers["sapi-result-type"] = value.GetType().Serialize();

        if (value is IResult result)
            return result;

        if (value is IStreamFileReadOnly file)
            return file.ToResult(ctx.Response, _options.JsonSerialization);

        if (value is Stream stream)
            return Results.Stream(stream);

        return Results.Json(value, _options.JsonSerialization);
    }

}
