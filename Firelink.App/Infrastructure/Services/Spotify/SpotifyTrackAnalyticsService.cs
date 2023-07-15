using System.Drawing;
using Firelink.App.Shared;
using Firelink.Application.Common.Interfaces;
using Firelink.Infrastructure.Common.Colors;
using Firelink.Infrastructure.Common.Track;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;
using SpotifyAPI.Web;

namespace Firelink.Infrastructure.Services.Spotify;

public class SpotifyTrackAnalyticsService : ISpotifyTrackAnalyticsService
{
    private readonly IAppCache _cache;
    private readonly ISpotifyApi _spotifyApi;

    public SpotifyTrackAnalyticsService(ISpotifyApi spotifyApi, IAppCache cache)
    {
        _spotifyApi = spotifyApi;
        _cache = cache;
    }

    public async Task<TrackDto?> GetTrackWithFeatures(CancellationToken cancellationToken)
    {
        try
        {
            var track = await _spotifyApi.GetCurrentTrack(cancellationToken);
            if (track == null)
                return null;
            
            return await _cache.GetOrAddAsync(track.Id, async entry =>
            {
                var feturesTask = GetFeatures(track.Id);
                var analysisTask = GetAnalysis(track.Id);
                var colorTask = ColorScraper.ScrapeColorForAlbum(track.Album.ExternalUrls["spotify"]);

                await Task.WhenAll(feturesTask, analysisTask, colorTask);
                var color = colorTask.Result;
                var trackDto = MapTrackDto(track, color, analysisTask.Result, feturesTask.Result);
                return trackDto;
            }, new MemoryCacheEntryOptions { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10) });
        }
        catch (Exception)
        {
            return new TrackDto();
        }
    }

    private async Task<TrackAudioFeatures> GetFeatures(string trackId)
    {
        return await _spotifyApi.Client.Tracks.GetAudioFeatures(trackId);
    }

    private async Task<TrackAudioAnalysis> GetAnalysis(string trackId)
    {
        return await _spotifyApi.Client.Tracks.GetAudioAnalysis(trackId);
    }

    private static TrackDto MapTrackDto(FullTrack track, Color color, TrackAudioAnalysis analysis, TrackAudioFeatures features)
    {
        return new TrackDto
        {
            Id = track.Id,
            Name = track.Name,
            Color = color,
            RGBColor = $"{color.R},{color.G},{color.B}",
            HsvColor = Hsv.ConvertToHSV(color),
            Album = MapAlbumDto(track),
            Artists = MapArtitsDto(track),
            Levels = WaveForm.FromTrackAnalysis(analysis),
            Tempo = features.Tempo,
            Duration = features.DurationMs,
            Energy = features.Energy
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
            Name = track.Artists.First().Name
        };
    }
}