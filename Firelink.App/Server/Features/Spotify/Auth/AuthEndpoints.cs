
using Firelink.App.Server.Extensions;
using Firelink.App.Shared;
using Firelink.Application.Auth.Commands.AuthenticateUserCommand;
using Firelink.Application.Auth.Queries.GetLoginUri;
using Firelink.Application.Auth.Queries.GetUserLoginStatus;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Firelink.App.Server.Features.Spotify.Auth;

public class AuthEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        var endpointGroup = app.MapGroup("auth");
        app.MapGet("/login", GotoLogin);
        endpointGroup.MapGet("/token", LoginWithToken);
        endpointGroup.MapGet("", IsUserLoggedIn);
    }

    private static async Task<Results<Ok<ResultResponse<bool>>, BadRequest>> IsUserLoggedIn(IMediator mediator)
    {
        //var isLoggedIn = await mediator.Send(GetUserLoginStatusQuery.Default);
        return TypedResults.Ok(new ResultResponse<bool>(true, true));
    }

    private static async Task<IResult> LoginWithToken(IMediator mediator, string code)
    {
        await mediator.Send(new AuthenticateUserCommand(code));
        return Results.Redirect("/");
    }

    private static async Task<IResult> GotoLogin(IMediator mediator)
    {
        var uri = await mediator.Send(GetLoginUriQuery.Default);
        return Results.Redirect(uri.ToString());
    }
    
}