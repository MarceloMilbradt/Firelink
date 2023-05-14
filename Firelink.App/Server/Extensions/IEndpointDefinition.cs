namespace Firelink.App.Server;

public interface IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app);
}