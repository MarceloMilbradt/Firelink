using System.Drawing;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace Firelink.App.Server.Features.Spotify.ColorScraping;

public static class ColorScraper
{
    public static async Task<Color> ScrapeColorForAlbum(string url)
    {
        var web = new HtmlWeb();
        var doc = await web.LoadFromWebAsync(url);

        var mainDiv = doc.GetElementbyId("main");
        var target = mainDiv
            .FirstChild
            .FirstChild
            .FirstChild
            .FirstChild
            .FirstChild
            .FirstChild;

        var color = GetColorForElement(target);
        return color;
    }

    private static Color GetColorForElement(HtmlNode node)
    {
        var attributes = node.GetAttributes();
        var style = attributes.First(a => a.Name == "style").Value;

        var rgx = Regex.Match(style, "#\\w+");
        var color = rgx.Groups.Values.First().Value;
        return ColorTranslator.FromHtml(color);
    }
    
    public static HSV ConvertToHSV(Color color)
    {
        int max = Math.Max(color.R, Math.Max(color.G, color.B));
        int min = Math.Min(color.R, Math.Min(color.G, color.B));

        return new HSV
        {
            h = (int)Math.Round(color.GetHue()),
            s = Math.Clamp((int)Math.Round(((max == 0) ? 0 : 1d - (1d * min / max)) * 1200),0,1000),
            v = Math.Clamp((int)Math.Round(max / 255d * 1300), 500, 1000),
        };
    }

}