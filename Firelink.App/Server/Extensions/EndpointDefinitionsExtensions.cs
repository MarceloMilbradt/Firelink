using Firelink.App.Server;

namespace Microsoft.Extensions.DependencyInjection;

public static class EndpointDefinitionsExtensions
{
    public static void AddEndpoints(this IServiceCollection services, params Type[] scanMarkers)
    {
        var endpointsDefinitions = new List<IEndpointDefinition>();
        foreach (var marker in scanMarkers)
        {
            endpointsDefinitions.AddRange(marker.Assembly.ExportedTypes.Where(x =>
                typeof(IEndpointDefinition).IsAssignableFrom(x) && x is { IsAbstract: false, IsInterface: false })
                .Select(Activator.CreateInstance).Cast<IEndpointDefinition>());
        }

        foreach (var endpointDefinition in endpointsDefinitions)
        {
            endpointDefinition.DefineServices(services);
        }

        services.AddSingleton(endpointsDefinitions as IReadOnlyCollection<IEndpointDefinition>);
    }

    public static void UseEndpoints(this WebApplication application)
    {
        var definitions = application.Services.GetRequiredService<IReadOnlyCollection<IEndpointDefinition>>();
        foreach (var endpointDefinition in definitions)
        {
            endpointDefinition.DefineEndpoints(application);
        }
    }
}