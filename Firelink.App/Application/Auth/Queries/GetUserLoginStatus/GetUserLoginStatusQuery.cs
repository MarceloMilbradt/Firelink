using Firelink.Application.Common.Interfaces;
using Mediator;

namespace Firelink.Application.Auth.Queries.GetUserLoginStatus;

public sealed record GetUserLoginStatusQuery : IRequest<bool>
{
    public static readonly GetUserLoginStatusQuery Default = new();
}


public sealed  class GetUserLoginStatusQueryHandler : IRequestHandler<GetUserLoginStatusQuery, bool>
{
    private readonly ISpotifyApi _spotifyApi;

    public GetUserLoginStatusQueryHandler(ISpotifyApi spotifyApi)
    {
        _spotifyApi = spotifyApi;
    }

    public ValueTask<bool> Handle(GetUserLoginStatusQuery request, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(_spotifyApi.IsUserLoggedIn());
    }
}