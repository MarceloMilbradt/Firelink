using Firelink.App.Shared.TrackConfiguration;
using Firelink.Application.Common.Interfaces;
using Mediator;

namespace Firelink.Application.Configuration.Commands.CreateConfiguration;

public record CreateConfigurationCommand(Uri Url, string Json) : IRequest;

public partial class CreateConfigurationCommandHandler : IRequestHandler<CreateConfigurationCommand>
{
    private readonly ISpotifyApi _spotifyApi;
    private readonly IWledConfigurationProvider _wledConfigurationProvider;
    public CreateConfigurationCommandHandler(ISpotifyApi spotifyApi, IWledConfigurationProvider wledConfigurationProvider)
    {
        _spotifyApi = spotifyApi;
        _wledConfigurationProvider = wledConfigurationProvider;
    }

    public async ValueTask<Unit> Handle(CreateConfigurationCommand request, CancellationToken cancellationToken)
    {

        string[] segments = request.Url.Segments;

        string type = segments[^2].Trim('/');
        string id = segments[^1].Trim('/');

        ConfigurationType configurationType = type switch
        {
            "track" => ConfigurationType.Track,
            "album" => ConfigurationType.Album,
            _ => throw new ArgumentException()
        };

        string title = configurationType switch
        {
            ConfigurationType.Track => await GetTrackName(id, cancellationToken),
            ConfigurationType.Album => await GetAlbumName(id, cancellationToken),
            _ => string.Empty
        };

        var configuration = new ItemConfiguration
        {
            Title = title,
            AppliesTo = configurationType,
            ItemId = id,
            Command = request.Json,
        };
        await _wledConfigurationProvider.AddConfiguration(configuration, cancellationToken);
        return new Unit();
    }

    private async Task<string> GetAlbumName(string id, CancellationToken cancellationToken)
    {
        var album = await _spotifyApi.Client.Albums.Get(id, cancellationToken);
        return album.Name;
    }
    private async Task<string> GetTrackName(string id, CancellationToken cancellationToken)
    {
        var track = await _spotifyApi.Client.Tracks.Get(id, cancellationToken);
        return track.Name;
    }
}