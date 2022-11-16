using Microsoft.AspNetCore.Routing;
using SingleApi;
using System.Reflection;

namespace Microsoft.AspNetCore.Builder;

public static class SingleApiExtensions
{
    public static IEndpointConventionBuilder MapSingleApi(this IEndpointRouteBuilder builder, string route, SapiDelegate handler, IEnumerable<Type> types, Action<SapiOptions>? optionsConfigurator = null)
    {
        var pattern = $"{route.TrimEnd('/')}/{{type:required}}";
        var options = new SapiOptions(); optionsConfigurator?.Invoke(options);
        var sapi = new SapiEndpoint(handler, types, options);

        return new EndpointConventionBuilder(new[]
        {
            builder.MapGet(pattern, sapi.ProcessGet),
            builder.MapPost(pattern, sapi.ProcessPost),
        });
    }

    public static IEndpointConventionBuilder MapSingleApi(this IEndpointRouteBuilder builder, string route, SapiDelegate handler, params Type[] types)
    {
        return MapSingleApi(builder, route, handler, types.AsEnumerable());
    }

    public static IEndpointConventionBuilder MapSingleApi(this IEndpointRouteBuilder builder, string route, SapiDelegate handler, params Assembly[] assemblies)
    {
        if (!assemblies.Any())
            assemblies = new[] { Assembly.GetCallingAssembly(), typeof(List<>).Assembly, typeof(int).Assembly };

        return MapSingleApi(builder, route, handler, assemblies
            .SelectMany(x => x.GetTypes())
            .Where(x => !x.IsAbstract && !x.IsAttribute() && !x.IsException()));
    }
}