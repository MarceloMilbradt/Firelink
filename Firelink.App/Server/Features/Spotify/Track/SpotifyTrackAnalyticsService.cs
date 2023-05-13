using System.Drawing;
using Firelink.App.Server.Features.Spotify.ColorScraping;
using Firelink.App.Shared;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;
using SpotifyAPI.Web;

namespace Firelink.App.Server.Features.Spotify.Track;

public class SpotifyTrackAnalyticsService
{
    private readonly IAppCache _cache;
    private readonly SpotifyTrackService _trackService;

    public SpotifyTrackAnalyticsService(SpotifyTrackService trackService)
    {
        _trackService = trackService;
        _cache = new CachingService();
    }

    public async Task<TrackDto> GetTrack()
    {
        try
        {
            var track = await _trackService.GetTrack();
            return await _cache.GetOrAddAsync(track.Id, async entry =>
            {
                var feturesTask = _trackService.GetFeatures(track);
                var analysisTask = _trackService.GetAnalysis(track);
                var colorTask = ColorScraper.ScrapeColorForAlbum(track.Album.ExternalUrls["spotify"]);

                await Task.WhenAll(feturesTask, analysisTask, colorTask);
                var color = colorTask.Result;
                var trackDto = MapTrackDto(track, color, analysisTask);
                return trackDto;
            }, new MemoryCacheEntryOptions { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10) });
        }
        catch (Exception)
        {
            return new TrackDto();
        }
    }

    private static TrackDto MapTrackDto(FullTrack track, Color color, Task<TrackAudioAnalysis> analysisTask)
    {
        return new TrackDto
        {
            Name = track.Name,
            Color = color,
            RGBColor = $"{color.R},{color.G},{color.B}",
            Album = MapAlbumDto(track),
            Artists = MapArtitsDto(track),
            Levels = WaveForm.FromTrackAnalysis(analysisTask.Result),
                    
        };
    }

    private static AlbumDto MapAlbumDto(FullTrack track)
    {
        return new AlbumDto
        {
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