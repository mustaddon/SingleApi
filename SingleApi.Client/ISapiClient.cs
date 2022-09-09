using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SingleApi
{
    public interface ISapiClient
    {
        Task<object?> Send(object? request, Type requestType, Type resultType, CancellationToken cancellationToken = default);
    }

    public static class SapiClientExtensions
    {
        public static Task<object?> Send(this ISapiClient client, object request, Type resultType, CancellationToken cancellationToken = default)
        {
            return client.Send(request ?? throw new ArgumentNullException(nameof(request)),
                requestType: request.GetType(),
                resultType: resultType,
                cancellationToken: cancellationToken);
        }

        public static async Task<TResult?> Send<TRequest, TResult>(this ISapiClient client, TRequest? request, CancellationToken cancellationToken = default)
        {
            var result = await client.Send(request,
                requestType: typeof(TRequest),
                resultType: typeof(TResult),
                cancellationToken: cancellationToken);

            if (result is TResult typed)
                return typed;

            return (TResult?)result;
        }

        public static async Task<TResult?> Send<TResult>(this ISapiClient client, IRequest<TResult> request, CancellationToken cancellationToken = default)
        {
            var result = await client.Send(request ?? throw new ArgumentNullException(nameof(request)),
                requestType: request.GetType(),
                resultType: typeof(TResult),
                cancellationToken: cancellationToken);

            if (result is TResult typed)
                return typed;

            return (TResult?)result;
        }

    }
}
