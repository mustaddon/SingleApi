using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace SingleApi
{
    internal static class HttpExtensions
    {

        public static ISapiFile? ToSapiFile(this HttpRequest httpRequest, Type type)
        {
            if (Activator.CreateInstance(type) is not ISapiFile instance)
                return null;

            instance.Content = httpRequest.Body;
            instance.Size = httpRequest.ContentLength ?? -1;

            if (ContentDispositionHeaderValue.TryParse(httpRequest.Headers.ContentDisposition, out var contentDisposition))
                instance.Name = contentDisposition.FileName;

            if (MediaTypeHeaderValue.TryParse(httpRequest.Headers.ContentType, out var contentType))
                instance.Type = contentType.MediaType;

            return instance;
        }

        public static IResult ToResult(this ISapiFile file, HttpResponse httpResponse)
        {
            var dispositionType = (file as ISapiFileResult)?.Inline == true ? DispositionTypeNames.Inline 
                : DispositionTypeNames.Attachment;

            httpResponse.Headers.ContentDisposition = new ContentDispositionHeaderValue(dispositionType)
            {
                FileName = file.Name,
            }.ToString();

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
