using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SingleApi
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
            using var response = await _client.Value.PostAsJsonAsync(Serialize(requestType), request,
                options: _settings.JsonSerializerOptions,
                cancellationToken: cancellationToken);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync(resultType, _settings.JsonSerializerOptions, cancellationToken);
        }

        private static string Serialize(Type type)
        {
            if (type.IsArray)
#pragma warning disable CS8604 // Possible null reference argument.
                return $"{nameof(Array)}({Serialize(type.GetElementType())})";
#pragma warning restore CS8604 // Possible null reference argument.

            if (!type.IsGenericType)
                return type.Name;

            return $"{type.Name.Split('`').First()}({string.Join(",", type.GenericTypeArguments.Select(x => Serialize(x)))})";
        }

        private HttpClient CreateClient(string address)
        {
            var handler = _settings.Credentials != null
                ? new HttpClientHandler { Credentials = _settings.Credentials }
                : new HttpClientHandler { UseDefaultCredentials = true };

            var client = new HttpClient(handler) { BaseAddress = new Uri(address.TrimEnd('/')+'/') };

            foreach (var kvp in _settings.DefaultRequestHeaders)
                client.DefaultRequestHeaders.Add(kvp.Key, kvp.Value);

            return client;
        }
    }
}
