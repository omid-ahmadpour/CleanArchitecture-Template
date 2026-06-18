using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace CleanTemplate.Dispatching
{
    public sealed class Dispatcher : IDispatcher
    {
        private static readonly MethodInfo SendCoreMethod = typeof(Dispatcher)
            .GetMethod(nameof(SendCore), BindingFlags.Instance | BindingFlags.NonPublic);

        private readonly IServiceProvider _serviceProvider;

        public Dispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var requestType = request.GetType();
            var method = SendCoreMethod.MakeGenericMethod(requestType, typeof(TResponse));

            return (Task<TResponse>)method.Invoke(this, new object[] { request, cancellationToken });
        }

        private async Task<TResponse> SendCore<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
            where TRequest : IRequest<TResponse>
        {
            var handlers = _serviceProvider.GetServices<IRequestHandler<TRequest, TResponse>>().ToArray();

            if (handlers.Length == 0)
            {
                throw new InvalidOperationException($"No request handler registered for '{typeof(TRequest).FullName}'.");
            }

            if (handlers.Length > 1)
            {
                throw new InvalidOperationException($"Multiple request handlers registered for '{typeof(TRequest).FullName}'.");
            }

            RequestHandlerDelegate<TResponse> handlerDelegate = () => handlers[0].Handle(request, cancellationToken);
            var behaviors = _serviceProvider.GetServices<IRequestPipelineBehavior<TRequest, TResponse>>().Reverse();

            foreach (var behavior in behaviors)
            {
                var next = handlerDelegate;
                handlerDelegate = () => behavior.Handle(request, next, cancellationToken);
            }

            return await handlerDelegate();
        }
    }
}
