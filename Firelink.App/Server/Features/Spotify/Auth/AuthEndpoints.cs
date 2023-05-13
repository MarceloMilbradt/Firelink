
using Firelink.App.Shared;
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

    private static async Task<Ok<ResultResponse<bool>>> IsUserLoggedIn(SpotifyAuthService authService)
    {
        var isLoggedIn = await authService.IsLoggedIn();
        return TypedResults.Ok(new ResultResponse<bool>(isLoggedIn, true));
    }

    private static async Task LoginWithToken(SpotifyAuthService authService, string code)
    {
        await authService.LogIn(code);
        Results.Redirect("/");
    }

    private static IResult GotoLogin(SpotifyAuthService authService)
    {
        return Results.Redirect(authService.GetLoginUri().ToString());
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddTransient<SpotifyAuthService>();
    }
}