using Firelink.Application.Common.Interfaces;
using MediatR;

namespace Firelink.Application.Auth.Queries.GetUserLoginStatus;

public sealed record GetUserLoginStatusQuery : IRequest<bool>
{
    public static readonly GetUserLoginStatusQuery Default = new();
}


internal sealed  class GetUserLoginStatusQueryHandler : IRequestHandler<GetUserLoginStatusQuery, bool>
{
    private readonly ISpotifyApi _spotifyApi;

    public GetUserLoginStatusQueryHandler(ISpotifyApi spotifyApi)
    {
        _spotifyApi = spotifyApi;
    }

    public Task<bool> Handle(GetUserLoginStatusQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_spotifyApi.IsUserLoggedIn());
    }
}