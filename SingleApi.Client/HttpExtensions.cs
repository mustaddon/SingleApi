using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SingleApi.Client
{
    internal static class HttpExtensions
    {
        public static bool IsPlainText(this MediaTypeHeaderValue? value)
        {
            return string.Equals(value?.MediaType, "text/plain", StringComparison.InvariantCultureIgnoreCase);
        }

        public static async Task<ISapiFile?> ToSapiFile(this HttpResponseMessage response, Type type)
        {
            if (Activator.CreateInstance(type) is not ISapiFile file)
                return null;

            file.Content = new SapiStreamWrapper(await response.Content.ReadAsStreamAsync(), () => response.Dispose());
            file.Name = response.Content.Headers.ContentDisposition?.FileNameStar ?? response.Content.Headers.ContentDisposition?.FileName;
            file.Type = response.Content.Headers.ContentType?.MediaType;

            if (file is ISapiFileResponse fileResponse
                && Enum.TryParse<SapiFileDispositions>(response.Content.Headers.ContentDisposition?.DispositionType, true, out var disposition))
                fileResponse.Disposition = disposition;

            return file;
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
