using Firelink.Application.Common.Interfaces;
using MediatR;

namespace Firelink.Application.Auth.Queries.GetLoginUri;

public sealed record GetLoginUriQuery : IRequest<Uri>
{
    public readonly static GetLoginUriQuery Default = new();
}

internal sealed class GetLoginUriQueryHandler : IRequestHandler<GetLoginUriQuery, Uri>
{
    private readonly ISpotifyApi _spotifyApi;

    public GetLoginUriQueryHandler(ISpotifyApi spotifyApi)
    {
        _spotifyApi = spotifyApi;
    }

    public Task<Uri> Handle(GetLoginUriQuery request, CancellationToken cancellationToken)
    {
        var uri = _spotifyApi.GetLoginUri();
        return Task.FromResult(uri);
    }
}