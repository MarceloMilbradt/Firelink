using Firelink.Application.Common.Interfaces;
using Mediator;

namespace Firelink.Application.Auth.Queries.GetLoginUri;

public sealed record GetLoginUriQuery : IRequest<Uri>
{
    public readonly static GetLoginUriQuery Default = new();
}

public sealed class GetLoginUriQueryHandler : IRequestHandler<GetLoginUriQuery, Uri>
{
    private readonly ISpotifyApi _spotifyApi;

    public GetLoginUriQueryHandler(ISpotifyApi spotifyApi)
    {
        _spotifyApi = spotifyApi;
    }

    public ValueTask<Uri> Handle(GetLoginUriQuery request, CancellationToken cancellationToken)
    {
        var uri = _spotifyApi.GetLoginUri();
        return ValueTask.FromResult(uri);
    }
}