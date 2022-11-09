using MetaFile;
using MetaFile.Http;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SingleApi.Client;

internal static class HttpExtensions
{
    public static Task<HttpResponseMessage> PostAsStream(this HttpClient client, string uri, object? request, CancellationToken cancellationToken)
    {
        if (request is not Stream stream)
            throw new ArgumentNullException(nameof(request));

        var content = new StreamContent(stream);

        return client.PostAsync(uri, content, cancellationToken);
    }

    public static Task<HttpResponseMessage> PostAsFile(this HttpClient client, string uri, object? request, JsonSerializerOptions jsonOptions, CancellationToken cancellationToken)
    {
        if (request is not IStreamFileReadOnly file)
            throw new ArgumentNullException(nameof(request));

        return client.PostAsStreamFile(uri, file, jsonOptions, cancellationToken);
    }

    public static void EnsureSuccessStatusCodeDisposable(this HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
            return;

        response.Dispose();
        throw new HttpRequestException($"Response status code does not indicate success: {(int)response.StatusCode} ({response.ReasonPhrase})");
    }

    public static bool IsPlainText(this MediaTypeHeaderValue? value)
    {
        return string.Equals(value?.MediaType, "text/plain", StringComparison.InvariantCultureIgnoreCase);
    }


#if NETSTANDARD2_0
#pragma warning disable IDE0060 // Remove unused parameter
    public static Task<Stream> ReadAsStreamAsync(this HttpContent httpContent, CancellationToken cancellationToken) => httpContent.ReadAsStreamAsync();
    public static Task<string> ReadAsStringAsync(this HttpContent httpContent, CancellationToken cancellationToken) => httpContent.ReadAsStringAsync();
#pragma warning restore IDE0060 // Remove unused parameter
#endif

}
