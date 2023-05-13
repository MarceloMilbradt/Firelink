namespace Firelink.App.Server;

public interface IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app);
    public void DefineServices(IServiceCollection services);
}