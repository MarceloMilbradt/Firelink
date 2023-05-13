using System.Drawing;
using Firelink.App.Shared;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;
using SpotifyAPI.Web;

namespace Firelink.App.Server.Features.Spotify.Track;

public class SpotifyTrackService
{
    private readonly ISpotifyClient _spotify;
    private readonly IAppCache _cache;
    public SpotifyTrackService(ISpotifyAPI spotify)
    {
        _spotify = spotify.Client;
        _cache = new CachingService();
    }

    private static PlayerCurrentlyPlayingRequest CreateRequest()
    {
        return new PlayerCurrentlyPlayingRequest(PlayerCurrentlyPlayingRequest.AdditionalTypes.Track);
    }
    
    public async Task<FullTrack?> GetTrack()
    {
        var currentlyPlaying = await _spotify.Player.GetCurrentlyPlaying(CreateRequest());
        if (currentlyPlaying == null)
        {
            return null;
        }
    
        var track = (FullTrack)currentlyPlaying.Item;
        return track;
    }
    
    public async Task<TrackAudioFeatures> GetFeatures(FullTrack? track)
    {
        var currentTrack = track ?? await GetTrack();
        return await _spotify.Tracks.GetAudioFeatures(currentTrack.Id);
    }

    public async Task<TrackAudioAnalysis> GetAnalysis(FullTrack? track)
    {
        var currentTrack = track ?? await GetTrack();
        return await _spotify.Tracks.GetAudioAnalysis(currentTrack.Id);
    }
    
  
}