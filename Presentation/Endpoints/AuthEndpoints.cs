using Firelink.Application.Auth.Commands.AuthenticateUserCommand;
using Firelink.Application.Auth.Queries.GetLoginUri;
using Mediator;

namespace Firelink.Presentation.Endpoints;

public static class AuthEndpoints 
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var endpointGroup = app.MapGroup("auth");
        app.MapGet("/login", GoToLogin);
        endpointGroup.MapGet("/token", LoginWithToken);
    }
    private static async Task<IResult> LoginWithToken(IMediator mediator, string code)
    {
        await mediator.Send(new AuthenticateUserCommand(code));
        return Results.Redirect("/");
    }

    private static async Task<IResult> GoToLogin(IMediator mediator)
    {
        var uri = await mediator.Send(GetLoginUriQuery.Default);
        return Results.Redirect(uri.ToString());
    }

}
