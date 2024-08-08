using Firelink.Application.Common.Interfaces;
using Firelink.Infrastructure.Common.Colors;
using Firelink.Infrastructure.Common.Persistence;
using System.Drawing;

namespace Firelink.Infrastructure.Services.Spotify;

internal class AlbumColorProvider : IAlbumColorProvider, IAsyncDisposable
{
    private const string JsonFilePath = "AlbumColors.json"; 
    private Dictionary<string, Color> _colors;

    public async ValueTask DisposeAsync()
    {
        await JsonFileManager.SaveToJson(JsonFilePath, _colors, default);
    }

    public async Task<Color> GetColorForAlbumUrl(string albumUrl, CancellationToken token)
    {
        _colors ??= await JsonFileManager.LoadFromJson<Dictionary<string, Color>>(JsonFilePath, token) ?? [];

        if (_colors.TryGetValue(albumUrl, out var color))
        {
            return color;
        }

        color = await ColorScraper.ScrapeColorForAlbum(albumUrl, token);

        _colors[albumUrl] = color;
        await JsonFileManager.SaveToJson(JsonFilePath, _colors, token);

        return color;
    }

}
