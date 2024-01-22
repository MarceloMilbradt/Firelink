using Firelink.App.Server.Extensions;
using Firelink.App.Shared.Devices;
using Firelink.App.Shared;
using Firelink.Application.Player.Commands.Listening;
using Firelink.Application.Player.Queries.Listening;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Firelink.App.Server.Features.Spotify.Player;

public class PlayerEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        var endpointGroup = app.MapGroup("player");
        endpointGroup.MapPost("stop", StopPlayer);
        endpointGroup.MapPost("play", StartPlayer);
        endpointGroup.MapGet("listening", IsListening);
    }

    private async Task<Results<Ok<ResultResponse<bool>>, BadRequest>> IsListening(IMediator mediator)
    {
        var result = await mediator.Send(request: new IsListeningQuery());
        return TypedResults.Ok(ResultResponse<bool>.Ok(result));
    }

    private async Task<Results<Ok, BadRequest>> StartPlayer(IMediator mediator)
    {
        await mediator.Send(request: SetPlayerListeningCommand.Listen);
        return TypedResults.Ok();
    }

    private async Task<Results<Ok, BadRequest>> StopPlayer(IMediator mediator)
    {
        await mediator.Send(request: SetPlayerListeningCommand.Stop);
        return TypedResults.Ok();
    }
}