using System;
using System.Net.Http.Headers;

namespace SingleApi.Client
{
    internal static class HttpExtensions
    {
        public static bool IsPlainText(this MediaTypeHeaderValue? value)
        {
            return string.Equals(value?.MediaType, "text/plain", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
