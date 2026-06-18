using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace CleanTemplate.Dispatching
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDispatching(this IServiceCollection services, params Assembly[] assemblies)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<IDispatcher, Dispatcher>();

            foreach (var assembly in assemblies.Where(a => a != null).Distinct())
            {
                foreach (var implementationType in assembly.GetTypes().Where(IsConcreteType))
                {
                    var handlerInterfaces = implementationType
                        .GetInterfaces()
                        .Where(IsRequestHandlerInterface)
                        .Distinct();

                    foreach (var handlerInterface in handlerInterfaces)
                    {
                        services.AddTransient(handlerInterface, implementationType);
                    }
                }
            }

            return services;
        }

        private static bool IsConcreteType(Type type)
        {
            return type.IsClass && !type.IsAbstract && !type.IsGenericTypeDefinition;
        }

        private static bool IsRequestHandlerInterface(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IRequestHandler<,>);
        }
    }
}
