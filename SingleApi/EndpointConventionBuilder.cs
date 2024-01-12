using Microsoft.AspNetCore.Builder;

namespace SingleApi;

internal class EndpointConventionBuilder(IEnumerable<IEndpointConventionBuilder> endpointConventionBuilders) : IEndpointConventionBuilder
{
    readonly IEnumerable<IEndpointConventionBuilder> _endpointConventionBuilders = endpointConventionBuilders;

    public void Add(Action<EndpointBuilder> convention)
    {
        foreach(var endpointBuilder in _endpointConventionBuilders)
            endpointBuilder.Add(convention);
    }
}