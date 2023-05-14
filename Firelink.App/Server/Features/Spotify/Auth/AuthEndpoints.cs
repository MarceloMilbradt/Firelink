
using Firelink.App.Shared;
using Firelink.Application.Auth.Commands.AuthenticateUserCommand;
using Firelink.Application.Auth.Queries.GetLoginUri;
using Firelink.Application.Auth.Queries.GetUserLoginStatus;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Firelink.App.Server.Features.Spotify.Auth;

public class AuthEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/login", GotoLogin);
        app.MapGet("/auth/token", LoginWithToken);
        app.MapGet("/auth", IsUserLoggedIn);
    }

    private static async Task<Ok<ResultResponse<bool>>> IsUserLoggedIn(IMediator mediator)
    {
        var isLoggedIn = await mediator.Send(GetUserLoginStatusQuery.Default);
        return TypedResults.Ok(new ResultResponse<bool>(isLoggedIn, true));
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