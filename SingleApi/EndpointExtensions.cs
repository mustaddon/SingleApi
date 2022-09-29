using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SingleApi;
using System.Reflection;
using System.Text.Json;

namespace Microsoft.AspNetCore.Builder
{
    public static class SingleApiExtensions
    {
        public static IEndpointConventionBuilder MapSingleApi(this IEndpointRouteBuilder builder, string route, SapiDelegate handler, params Assembly[] assemblies)
        {
            if (!assemblies.Any())
                assemblies = new[] { Assembly.GetCallingAssembly(), typeof(List<>).Assembly, typeof(int).Assembly };

            return MapSingleApi(builder, route, handler, assemblies.SelectMany(x => x.GetTypes()));
        }

        public static IEndpointConventionBuilder MapSingleApi(this IEndpointRouteBuilder builder, string route, SapiDelegate handler, IEnumerable<Type> types, JsonSerializerOptions? jsonOptions = null)
        {
            var pattern = $"{route.TrimEnd('/')}/{{type:required}}";
            var sapi = new SapiEndpoint(types, jsonOptions);

            return new EndpointConventionBuilder(new []
            {
                builder.MapGet(pattern, (HttpContext context, [FromRoute]string type, [FromQuery]string? data, CancellationToken cancellationToken)
                    => sapi.ProcessGet(context, type, data, handler, cancellationToken)),

                builder.MapPost(pattern, (HttpContext context, [FromRoute]string type, CancellationToken cancellationToken)
                    => sapi.ProcessPost(context, type, handler, cancellationToken)),
            });
        }
    }
}