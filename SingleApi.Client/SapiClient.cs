using MetaFile;
using MetaFile.Http;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using TypeSerialization;

namespace SingleApi.Client;

public class SapiClient : ISapiClient, IDisposable
{
    public SapiClient(string address, SapiClientSettings? settings = null)
    {
        _settings = settings ?? SapiClientSettings.Default;
        _client = new(() => CreateClient(address));
    }

    private readonly SapiClientSettings _settings;
    private readonly Lazy<HttpClient> _client;


    public HttpRequestHeaders DefaultRequestHeaders => _client.Value.DefaultRequestHeaders;

    public void Dispose()
    {
        if (_client.IsValueCreated)
            _client.Value.Dispose();

        GC.SuppressFinalize(this);
    }

    public virtual Task<object?> Send(object? request, Type requestType, Type resultType, CancellationToken cancellationToken = default)
    {
        return GetResult(CreateRequest(request, requestType, cancellationToken), resultType, cancellationToken);
    }

    protected virtual Task<HttpResponseMessage> CreateRequest(object? request, Type requestType, CancellationToken cancellationToken)
    {
        if (typeof(Stream).IsAssignableFrom(requestType))
            return _client.Value.PostAsStream(typeof(Stream).Serialize(), request, cancellationToken);

        if (typeof(IStreamFileReadOnly).IsAssignableFrom(requestType))
            return _client.Value.PostAsFile(requestType.Serialize(), request, _settings.JsonSerializerOptions, cancellationToken);

        return _client.Value.PostAsJsonAsync(requestType.Serialize(), request, _settings.JsonSerializerOptions, cancellationToken);
    }

    protected virtual async Task<object?> GetResult(Task<HttpResponseMessage> requestTask, Type resultType, CancellationToken cancellationToken)
    {
        var response = await requestTask;

        response.EnsureSuccessStatusCodeDisposable();

        if (response.StatusCode == HttpStatusCode.NoContent || resultType.Equals(Types.Void))
            return null;

        if (resultType.Equals(Types.Object) && response.Headers.TryGetValues("sapi-result-type", out var resultTypeHeaders))
            resultType = _settings.TypeDeserializer.Deserialize(resultTypeHeaders.First());

        if (resultType.Equals(Types.Stream))
            return new StreamWrapper(await response.Content.ReadAsStreamAsync(cancellationToken), () => response.Dispose());

        if (!resultType.Equals(Types.Object))
        {
            if (resultType.IsAssignableFrom(Types.StreamFile))
                return await response.ReadAsStreamFile(Types.StreamFile, _settings.JsonSerializerOptions, cancellationToken);

            if (!resultType.IsAbstract && Types.IStreamFile.IsAssignableFrom(resultType))
                return await response.ReadAsStreamFile(resultType, _settings.JsonSerializerOptions, cancellationToken);
        }

        using (response)
        {
            if (resultType == typeof(string) && response.Content.Headers.ContentType.IsPlainText())
                return await response.Content.ReadAsStringAsync(cancellationToken);

            return await response.Content.ReadFromJsonAsync(resultType, _settings.JsonSerializerOptions, cancellationToken);
        }
    }

    protected virtual HttpClient CreateClient(string address)
    {
        if (!address.EndsWith("/"))
            address += "/";

        var handler = _settings.Credentials != null
            ? new HttpClientHandler { Credentials = _settings.Credentials }
            : new HttpClientHandler { UseDefaultCredentials = true };

        var client = new HttpClient(handler) { BaseAddress = new Uri(address) };

        foreach (var kvp in _settings.DefaultRequestHeaders)
            client.DefaultRequestHeaders.Add(kvp.Key, kvp.Value);

        return client;
    }
}
