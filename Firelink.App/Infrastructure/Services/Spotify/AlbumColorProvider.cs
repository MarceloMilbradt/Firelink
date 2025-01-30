using Firelink.Application.Common.Interfaces;
using Firelink.Infrastructure.Common.Colors;
using Firelink.Infrastructure.Common.Persistence;
using System.Drawing;

namespace Firelink.Infrastructure.Services.Spotify;

internal class AlbumColorProvider : IAlbumColorProvider, IAsyncDisposable
{
    private const string JsonFilePath = "AlbumColors";
    private Dictionary<string, SerializableColor>? _colors;

    public async ValueTask DisposeAsync()
    {
        if (_colors != null && _colors.Any())
        {
            await MemoryPackFileManager.SaveToFile(JsonFilePath, _colors, default);
        }
    }

    public async Task<Color> GetColorForAlbumUrl(string albumUrl, CancellationToken token)
    {
        _colors ??= await MemoryPackFileManager.LoadFromFile<Dictionary<string, SerializableColor>>(JsonFilePath, token) ?? [];

        if (_colors.TryGetValue(albumUrl, out var color) && !((Color)color).IsEmpty && color != Color.Black)
        {
            return color;
        }

        color = await ColorScraper.ScrapeColorForAlbum(albumUrl, token);

        _colors[albumUrl] = color;
        if (_colors != null && _colors.Any())
        {
            await MemoryPackFileManager.SaveToFile(JsonFilePath, _colors, token);
        }
        return color;
    }

}
