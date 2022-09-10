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

            return MapSingleApi(builder, route, handler, assemblies.Distinct().ToArray().SelectMany(x => x.GetTypes()));
        }

        public static IEndpointConventionBuilder MapSingleApi(this IEndpointRouteBuilder builder, string route, SapiDelegate handler, IEnumerable<Type> types)
        {
            var pattern = route.TrimEnd('/') + "/{type:required}";
            var container = new SapiContainer(types);

            return new EndpointConventionBuilder(new List<IEndpointConventionBuilder>()
            {
                builder.MapGet(pattern, (HttpContext context, [FromRoute]string type, [FromQuery]string data, CancellationToken cancellationToken)
                    => container.Get(context, type, data, handler, cancellationToken)),

                builder.MapPost(pattern, (HttpContext context, [FromRoute]string type, [FromBody]JsonElement json, CancellationToken cancellationToken)
                    => container.Post(context, type, json, handler, cancellationToken)),
            });
        }
    }
}