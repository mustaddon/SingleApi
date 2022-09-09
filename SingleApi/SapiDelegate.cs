using Microsoft.AspNetCore.Http;

namespace SingleApi
{
    public delegate Task<object?> SapiDelegate(SapiDelegateArgs args);

    public class SapiDelegateArgs
    {
        internal SapiDelegateArgs(HttpContext httpContext, Type dataType, object? data, CancellationToken cancellationToken)
        {
            HttpContext = httpContext;
            DataType = dataType;
            Data = data;
            CancellationToken = cancellationToken;
        }

        public IServiceProvider ServiceProvider => this.HttpContext.RequestServices;
        public HttpContext HttpContext { get; }
        public Type DataType { get; }
        public object? Data { get; }
        public CancellationToken CancellationToken { get; }
    }
}
