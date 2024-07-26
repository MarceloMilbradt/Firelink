using System.Drawing;
using System.Text.RegularExpressions;
using Firelink.App.Shared;
using HtmlAgilityPack;

namespace Firelink.Infrastructure.Common.Colors;

public static partial class ColorScraper
{
    public static async Task<Color> ScrapeColorForAlbum(string url)
    {
        var web = new HtmlWeb();
        var doc = await web.LoadFromWebAsync(url);

        var mainDiv = doc.GetElementbyId("main");
        var target = mainDiv
            .ChildNodes[1]
            .FirstChild
            .FirstChild
            .FirstChild
            .FirstChild
            .ChildNodes[1]
            .FirstChild;

        var color = GetColorForElement(target);
        return color;
    }

    private static Color GetColorForElement(HtmlNode node)
    {
        var attributes = node.GetAttributes();
        var style = attributes.First(a => a.Name == "style").Value;

        var rgx = ColorRegex().Match(style);
        var color = rgx.Groups.Values.First().Value;
        return ColorTranslator.FromHtml(color);
    }

    [GeneratedRegex("#\\w+")]
    private static partial Regex ColorRegex();
}