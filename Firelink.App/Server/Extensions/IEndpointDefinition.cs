namespace Firelink.App.Server.Extensions;

public interface IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app);
}