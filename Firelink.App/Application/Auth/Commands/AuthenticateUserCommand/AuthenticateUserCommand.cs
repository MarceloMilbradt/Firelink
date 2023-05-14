using Firelink.Application.Common.Interfaces;
using MediatR;

namespace Firelink.Application.Auth.Commands.AuthenticateUserCommand;

public sealed record AuthenticateUserCommand(string Code) : IRequest<bool>;

internal sealed class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, bool>
{
    private readonly ISpotifyApi _spotifyApi;

    public AuthenticateUserCommandHandler(ISpotifyApi spotifyApi)
    {
        _spotifyApi = spotifyApi;
    }

    public async Task<bool> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        await _spotifyApi.Connect(request.Code, cancellationToken);
        return true;
    }
}