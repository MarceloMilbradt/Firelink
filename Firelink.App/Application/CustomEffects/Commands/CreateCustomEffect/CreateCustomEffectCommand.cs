using Firelink.Domain.CustomEffects;
using Firelink.Application.Common.Interfaces;
using Mediator;
using OneOf.Types;
using OneOf;
using Firelink.Application.Common.Result;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace Firelink.Application.CustomEffects.Commands.CreateCustomEffect;

public sealed record CreateCustomEffectCommand(Uri Url, int PaletteId, int PresetId, int EffectId) : IRequest<OneOf<Success, NotASpotifyUrl, Error>>
{
    public static CreateCustomEffectCommand FromEffect(int effectId, int PaletteId, Uri uri) => new(uri, PaletteId, 0, effectId);
    public static CreateCustomEffectCommand FromPreset(int presetId, Uri uri) => new(uri, 0, presetId, 0);
}

public partial class CreateCustomEffectCommandHandler : IRequestHandler<CreateCustomEffectCommand, OneOf<Success, NotASpotifyUrl, Error>>
{
    private readonly ISpotifyApi _spotifyApi;
    private readonly IWledCustomEffectProvider _wledConfigurationProvider;
    private readonly ILogger<CreateCustomEffectCommandHandler> _logger;
    public CreateCustomEffectCommandHandler(ISpotifyApi spotifyApi, IWledCustomEffectProvider wledConfigurationProvider, ILogger<CreateCustomEffectCommandHandler> logger)
    {
        _spotifyApi = spotifyApi;
        _wledConfigurationProvider = wledConfigurationProvider;
        _logger = logger;
    }

    public async ValueTask<OneOf<Success, NotASpotifyUrl, Error>> Handle(CreateCustomEffectCommand request, CancellationToken cancellationToken)
    {
        if (!IsValidSpotifyUrl(request.Url))
        {
            return new NotASpotifyUrl();
        }

        string[] segments = request.Url.Segments;

        string type = segments[^2].Trim('/');
        string id = segments[^1].Trim('/');

        string title = type switch
        {
            "track" => await GetTrackName(id, cancellationToken),
            "album" => await GetAlbumName(id, cancellationToken),
            _ => throw new InvalidOperationException()
        };


        var configurationType = request switch
        {
            { PresetId: > 0 } => ConfigurationType.Preset,
            { EffectId: > 0 } => ConfigurationType.Effect,
            _ => throw new InvalidOperationException(),
        };

        var configuration = new CustomEffect
        {
            Title = title,
            EffectType = configurationType,
            ItemId = id,
            Url = request.Url.ToString(),
            PresetId = request.PresetId,
            EffectId = request.EffectId,
            PaletteId = request.PaletteId,
        };

        try
        {
            await _wledConfigurationProvider.AddCustomEffect(configuration, cancellationToken);
            return new Success();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Creating Effect");
            return new Error();
        }
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

    public static bool IsValidSpotifyUrl(Uri url)
    {
        return UrlRegex().IsMatch(url.ToString());
    }


    [GeneratedRegex("^https:\\/\\/open\\.spotify\\.com\\/(intl-[a-z]{2}\\/)?(track|album)\\/[a-zA-Z0-9]{22}(\\?.*)?$")]
    private static partial Regex UrlRegex();
}
