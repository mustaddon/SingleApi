using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;

namespace SingleApi
{
    internal static class HttpExtensions
    {

        public static ISapiFile? ToSapiFile(this HttpRequest httpRequest, Type type, JsonSerializerOptions jsonOptions)
        {
            if (Activator.CreateInstance(type) is not ISapiFile file)
                return null;

            file.Content = new SapiRequestStream(httpRequest);

            if (MediaTypeHeaderValue.TryParse(httpRequest.Headers.ContentType, out var contentType))
                file.Type = contentType.MediaType;

            if (ContentDispositionHeaderValue.TryParse(httpRequest.Headers.ContentDisposition, out var contentDisposition))
                file.Name = contentDisposition.FileNameStar ?? contentDisposition.FileName;

            var fileType = file.GetType();

            if (httpRequest.Headers.ContainsKey(HeaderNames.Metadata))
            {
                var metadataType = fileType.GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ISapiFile<>))?.GenericTypeArguments.First();

                if (metadataType != null)
                {
                    var metadata = JsonSerializer.Deserialize(Uri.UnescapeDataString(httpRequest.Headers[HeaderNames.Metadata]), metadataType, jsonOptions);
                    var metadataProp = fileType.GetProperty(nameof(ISapiFile<int>.Metadata));
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    metadataProp.SetValue(file, metadata);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                }
            }

            return file;
        }

        public static IResult ToResult(this ISapiFile file, HttpResponse httpResponse, JsonSerializerOptions jsonOptions)
        {
            var dispositionType = (file as ISapiFileResponse)?.Disposition == SapiFileDispositions.Inline
                ? DispositionTypeNames.Inline
                : DispositionTypeNames.Attachment;

            httpResponse.Headers.ContentDisposition = new ContentDispositionHeaderValue(dispositionType)
            {
                FileName = file.Name,
                FileNameStar = file.Name,
            }.ToString();

            var fileType = file.GetType();

            if (fileType.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ISapiFile<>)))
            {
                var metadataProp = fileType.GetProperty(nameof(ISapiFile<int>.Metadata));
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                httpResponse.Headers[HeaderNames.Metadata] = Uri.EscapeDataString(JsonSerializer.Serialize(metadataProp.GetValue(file), metadataProp.PropertyType, jsonOptions));
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }

            return Results.Stream(file.Content, file.Type);
        }

        public static IHeaderDictionary AddNoCache(this IHeaderDictionary headers)
        {
            foreach (var kvp in NoCacheHeaders)
                headers[kvp.Key] = kvp.Value;

            return headers;
        }

        static readonly Dictionary<string, string> NoCacheHeaders = new()
        {
            { "Cache-Control", "no-cache, no-store, must-revalidate" },
            { "Pragma", "no-cache" },
            { "Expires", "0" },
        };
    }
}
