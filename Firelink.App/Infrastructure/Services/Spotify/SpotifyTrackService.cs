using System.Drawing;
using Firelink.Domain;
using Firelink.Application.Common.Interfaces;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;
using SpotifyAPI.Web;
using Microsoft.Extensions.Logging;

namespace Firelink.Infrastructure.Services.Spotify;

public class SpotifyTrackService : ISpotifyTrackService
{
    private readonly IAppCache _cache;
    private readonly ISpotifyApi _spotifyApi;
    private readonly IAlbumColorProvider _albumColorProvider;
    private ILogger<SpotifyTrackService> _logger;
    public SpotifyTrackService(ISpotifyApi spotifyApi, IAppCache cache, IAlbumColorProvider albumColorProvider, ILogger<SpotifyTrackService> logger)
    {
        _spotifyApi = spotifyApi;
        _cache = cache;
        _albumColorProvider = albumColorProvider;
        _logger = logger;
    }

    public async Task<TrackDto?> GetTrackWithColor(CancellationToken cancellationToken)
    {
        var track = await _spotifyApi.GetCurrentTrack(cancellationToken);
        if (track == null)
            return null;

        var trackDto = await _cache.GetOrAddAsync(track.Id, async entry =>
        {
            var colorTask = await _albumColorProvider.GetColorForAlbumUrl(track.Album.ExternalUrls["spotify"], cancellationToken);

            var trackDto = MapTrackDto(track, colorTask);
            return trackDto;
        }, new MemoryCacheEntryOptions { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10) });
        return trackDto;
    }



    private static TrackDto MapTrackDto(FullTrack track, Color color)
    {
        return new TrackDto
        {
            Id = track.Id,
            Name = track.Name,
            Color = color,
            RGBColor = $"{color.R},{color.G},{color.B}",
            Album = MapAlbumDto(track),
            Artists = MapArtitsDto(track),
        };
    }

    private static AlbumDto MapAlbumDto(FullTrack track)
    {
        return new AlbumDto
        {
            Id = track.Album.Id,
            Name = track.Album.Name,
            Image = new ImageDto
            {
                Url = track.Album.Images.First().Url
            },
            ReleaseDate = track.Album.ReleaseDate
        };
    }

    private static ArtitsDto MapArtitsDto(FullTrack track)
    {
        return new ArtitsDto
        {
            Name = track.Artists.First().Name,

        };
    }
}