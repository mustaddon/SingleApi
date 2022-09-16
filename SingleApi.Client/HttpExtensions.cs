using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;

namespace SingleApi.Client
{
    internal static class HttpExtensions
    {
        public static bool IsPlainText(this MediaTypeHeaderValue? value)
        {
            return string.Equals(value?.MediaType, "text/plain", StringComparison.InvariantCultureIgnoreCase);
        }

        public static async Task<SapiFile> ToFileResult(this HttpResponseMessage response)
        {
            return new SapiFile(new SapiStreamWrapper(await response.Content.ReadAsStreamAsync(), () => response.Dispose()))
            {
                Name = response.Content.Headers.ContentDisposition?.FileName,
                Type = response.Content.Headers.ContentType?.MediaType,
                Inline = string.Equals(response.Content.Headers.ContentDisposition?.DispositionType, DispositionTypeNames.Inline, StringComparison.InvariantCultureIgnoreCase),
            };
        }

        public static void EnsureSuccessStatusCodeDisposable(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                response.Dispose();
                throw new HttpRequestException($"Response status code does not indicate success: {(int)response.StatusCode} ({response.ReasonPhrase})");
            }
        }
    }
}
