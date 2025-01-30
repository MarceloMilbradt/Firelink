using System.Drawing;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace Firelink.Infrastructure.Common.Colors;

public static partial class ColorScraper
{
    public static async Task<Color> ScrapeColorForAlbum(string url, CancellationToken cancellationToken)
    {
        try
        {
            var web = new HtmlWeb();
            web.CaptureRedirect = true;
            web.UseCookies = true;
           
            var doc = await web.LoadFromWebAsync(url, cancellationToken);
            

            var mainDiv = doc.GetElementbyId("main");
            var target = mainDiv.SelectNodes("//div[contains(@style, 'background:linear-gradient')]").FirstOrDefault();
            if(target == null)
            {
                target = mainDiv.SelectNodes("//div[contains(@style, 'background: linear-gradient')]").FirstOrDefault();
            }

            var color = GetColorForElement(target);
            return color;
        }
        catch (Exception)
        {
            return Color.Black;
        }
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