using SpotifyAPI.Web;

namespace Firelink.App.Shared;

public class WaveForm
{
    public struct Segment
    {
        public float start;
        public float duration;
        public float loudness;
    }

    public static IEnumerable<double> FromTrackAnalysis(TrackAudioAnalysis analysis)
    {
        var levels = new List<double>(1000);
        var trackDuration = analysis.Track.Duration;
        IEnumerable<Segment> segments = GetSegments(analysis, trackDuration);
        var max = segments.Max(l => l.loudness);
        MeasureLoudness(segments, levels, max);
        return levels;
    }

    private static void MeasureLoudness(IEnumerable<Segment> segments,
        ICollection<double> levels, float max)
    {
        for (var i = 0d; i < 1; i += 0.001)
        {
            var segment = segments.First(s => i <= s.start + s.duration);
            var loudness = Math.Round(segment.loudness / max * 100) / 100;
            levels.Add(loudness);
        }
    }

    private static IEnumerable<Segment> GetSegments(TrackAudioAnalysis analysis,  float trackDuration)
    {
        return analysis.Segments.Select(segment =>
        {
            var loudness = segment.LoudnessMax;
            var start = segment.Start / trackDuration;
            var duration = segment.Duration / trackDuration;
            return new Segment
            {
                start = start,
                duration = duration,
                loudness = 1 - (Math.Min(Math.Max(loudness, -35), 0) / -35),
            };
        });
    }
}