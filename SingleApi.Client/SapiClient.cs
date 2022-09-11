using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SingleApi.Client
{
    public class SapiClient : ISapiClient, IDisposable
    {
        public SapiClient(string address, SapiClientSettings? settings = null)
        {
            _settings = settings ?? new();
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

        public async Task<object?> Send(object? request, Type requestType, Type resultType, CancellationToken cancellationToken = default)
        {
            using var response = await _client.Value.PostAsJsonAsync(requestType.Serialize(), request,
                options: _settings.JsonSerializerOptions,
                cancellationToken: cancellationToken);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync(resultType, _settings.JsonSerializerOptions, cancellationToken);
        }

        private HttpClient CreateClient(string address)
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
}
