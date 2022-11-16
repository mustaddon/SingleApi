using Microsoft.AspNetCore.Builder;

namespace SingleApi;

internal class EndpointConventionBuilder : IEndpointConventionBuilder
{
    public EndpointConventionBuilder(IEnumerable<IEndpointConventionBuilder> endpointConventionBuilders)
    {
        _endpointConventionBuilders = endpointConventionBuilders;
    }

    readonly IEnumerable<IEndpointConventionBuilder> _endpointConventionBuilders;

    public void Add(Action<EndpointBuilder> convention)
    {
        foreach(var endpointBuilder in _endpointConventionBuilders)
            endpointBuilder.Add(convention);
    }
}