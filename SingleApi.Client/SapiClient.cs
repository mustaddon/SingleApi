﻿using System;
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

        public virtual Task<object?> Send(object? request, Type requestType, Type resultType, CancellationToken cancellationToken = default)
        {
            return GetResult(CreateRequest(request, requestType, cancellationToken), resultType, cancellationToken);
        }

        protected virtual Task<HttpResponseMessage> CreateRequest(object? request, Type requestType, CancellationToken cancellationToken)
        {
            return _client.Value.PostAsJsonAsync(requestType.Serialize(), request,
                options: _settings.JsonSerializerOptions,
                cancellationToken: cancellationToken);
        }

        protected virtual async Task<object?> GetResult(Task<HttpResponseMessage> requestTask, Type resultType, CancellationToken cancellationToken)
        {
            var response = await requestTask;

            response.EnsureSuccessStatusCodeDisposable();

            if (resultType == typeof(SapiFile))
                return await response.ToFileResult();

            using (response)
            {
                if (resultType == typeof(string) && response.Content.Headers.ContentType.IsPlainText())
                    return await response.Content.ReadAsStringAsync();

                return await response.Content.ReadFromJsonAsync(resultType, _settings.JsonSerializerOptions, cancellationToken);
            }
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
