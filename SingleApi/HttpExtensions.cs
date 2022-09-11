using Microsoft.AspNetCore.Http;

namespace SingleApi
{
    internal static class HttpExtensions
    {
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
