using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LSL.Sentinet.Tool.Cli.Infrastructure;

public static class AddHandlerInterceptorsServiceCollectionExtensions
{
    public static IServiceCollection AddHandlerInterceptors(this IServiceCollection services)
    {
        services.TryAddTransient<AsyncHandlerInterceptor>();

        var toDecorate = new List<ServiceDescriptor>();

        foreach (var service in services)
        {
            if (service.ServiceType.IsGenericType)
            {
                var genericTypeDefinition = service.ServiceType.GetGenericTypeDefinition();
                if (typeof(IExecuteCommandLineOptionsAsync<,>).GetGenericTypeDefinition() == genericTypeDefinition)
                {
                    toDecorate.Add(service);
                }
            }
        }

        foreach (var service in toDecorate)
        {
            services.DecorateWithInterceptors(service.ServiceType, new[] { typeof(AsyncHandlerInterceptor) }.AsEnumerable());
        }
        return services;
    }    
}