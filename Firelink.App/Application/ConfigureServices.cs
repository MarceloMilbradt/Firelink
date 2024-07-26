using System.Reflection;
using Firelink.Application.Common.Behavior;
using Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace Firelink.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));

        return services;
    }
}