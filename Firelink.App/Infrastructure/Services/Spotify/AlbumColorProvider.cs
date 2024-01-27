using Firelink.Application.Common.Interfaces;
using Firelink.Infrastructure.Common.Colors;
using Newtonsoft.Json;
using System.Drawing;

namespace Firelink.Infrastructure.Services.Spotify;

internal class AlbumColorProvider : IAlbumColorProvider, IAsyncDisposable
{
    private const string JsonFilePath = "AlbumColors.json"; 
    private Dictionary<string, Color> _colors;

    public async ValueTask DisposeAsync()
    {
        await SaveColorsToJson();
    }

    public async Task<Color> GetColorForAlbumUrl(string albumUrl)
    {
        // Load colors from JSON file if empty
        if (_colors == null)
        {
            await LoadColorsFromJson();
        }

        // Try to get the color from the dictionary
        if (_colors.TryGetValue(albumUrl, out var color))
        {
            return color;
        }

        // If not present, try to get the color from another source
        color = await ColorScraper.ScrapeColorForAlbum(albumUrl);

        // Save the color to the file and the dictionary
        _colors[albumUrl] = color;
        await SaveColorsToJson();

        return color;
    }

    private async Task LoadColorsFromJson()
    {
        try
        {
            if (File.Exists(JsonFilePath))
            {
                string json = await File.ReadAllTextAsync(JsonFilePath);
                _colors = JsonConvert.DeserializeObject<Dictionary<string, Color>>(json) ?? [];
            }
            else
            {
                _colors = [];
            }     
        }
        catch (Exception)
        {
            _colors = [];
        }
    }

    private async Task SaveColorsToJson()
    {
        string json = JsonConvert.SerializeObject(_colors);
        await File.WriteAllTextAsync(JsonFilePath, json);
    }

}
