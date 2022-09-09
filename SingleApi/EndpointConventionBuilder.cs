using Microsoft.AspNetCore.Builder;

namespace SingleApi
{
    public sealed class EndpointConventionBuilder : IEndpointConventionBuilder
    {
        internal EndpointConventionBuilder(List<IEndpointConventionBuilder> endpointConventionBuilders)
        {
            _endpointConventionBuilders = endpointConventionBuilders;
        }

        readonly List<IEndpointConventionBuilder> _endpointConventionBuilders;

        public void Add(Action<EndpointBuilder> convention)
        {
            _endpointConventionBuilders.ForEach(x => x.Add(convention));
        }
    }
}