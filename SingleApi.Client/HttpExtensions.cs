using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SingleApi.Client
{
    internal static class HttpExtensions
    {
        public static async Task<ISapiFile?> ToSapiFile(this HttpResponseMessage response, Type type, JsonSerializerOptions jsonOptions)
        {
            if (Activator.CreateInstance(type) is not ISapiFile file)
                return null;

            file.Content = new SapiStreamWrapper(await response.Content.ReadAsStreamAsync(), () => response.Dispose());
            file.Name = response.Content.Headers.ContentDisposition?.FileNameStar ?? response.Content.Headers.ContentDisposition?.FileName;
            file.Type = response.Content.Headers.ContentType?.MediaType;

            if (file is ISapiFileResponse fileResponse)
                fileResponse.InlineDisposition = string.Equals(DispositionTypeNames.Inline, response.Content.Headers.ContentDisposition?.DispositionType, StringComparison.InvariantCultureIgnoreCase);

            if (response.Headers.TryGetValues(SapiHeaders.Metadata, out var metadataValues) && metadataValues.Any())
            {
                var metadataType = type.GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ISapiFile<>))?.GenericTypeArguments.First();

                if (metadataType != null)
                {
                    var metadata = JsonSerializer.Deserialize(Uri.UnescapeDataString(metadataValues.First()), metadataType, jsonOptions);
                    var metadataProp = type.GetProperty(nameof(ISapiFile<int>.Metadata));
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    metadataProp.SetValue(file, metadata);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                }
            }

            return file;
        }

        public static Task<HttpResponseMessage> PostAsSapiFile(this HttpClient client, string uri, object? request, Type type, JsonSerializerOptions jsonOptions, CancellationToken cancellationToken)
        {
            if (request is not ISapiFile file)
                throw new ArgumentNullException(nameof(request));

            var content = new StreamContent(file.Content);

            if (file.Type != null)
                content.Headers.ContentType = new MediaTypeHeaderValue(file.Type);

            var dispositionType = (file as ISapiFileResponse)?.InlineDisposition == true
                ? DispositionTypeNames.Inline
                : DispositionTypeNames.Attachment;

            content.Headers.ContentDisposition = new ContentDispositionHeaderValue(dispositionType)
            {
                FileName = file.Name,
                FileNameStar = file.Name,
            };

            if (type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ISapiFile<>)))
            {
                var metadataProp = type.GetProperty(nameof(ISapiFile<int>.Metadata));
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                content.Headers.TryAddWithoutValidation(SapiHeaders.Metadata, Uri.EscapeDataString(JsonSerializer.Serialize(metadataProp.GetValue(file), metadataProp.PropertyType, jsonOptions)));
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }

            return client.PostAsync(uri, content, cancellationToken);
        }

        public static void EnsureSuccessStatusCodeDisposable(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                response.Dispose();
                throw new HttpRequestException($"Response status code does not indicate success: {(int)response.StatusCode} ({response.ReasonPhrase})");
            }
        }

        public static bool IsPlainText(this MediaTypeHeaderValue? value)
        {
            return string.Equals(value?.MediaType, "text/plain", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
