using System.Drawing;
using Newtonsoft.Json;

namespace Firelink.App.Shared;

public record  Hsv
{
    public int H;
    public int S;
    public int V;
    public override string ToString()
    {
        return H.ToString("X4").ToLower() + S.ToString("X4").ToLower() + V.ToString("X4").ToLower();
    }
    
    public static Hsv ConvertToHSV(Color color)
    {
        int max = Math.Max(color.R, Math.Max(color.G, color.B));
        int min = Math.Min(color.R, Math.Min(color.G, color.B));

        return new Hsv
        {
            H = (int)Math.Round(color.GetHue()),
            S = Math.Clamp((int)Math.Round(((max == 0) ? 0 : 1d - (1d * min / max)) * 1200),0,1000),
            V = Math.Clamp((int)Math.Round(max / 255d * 1300), 500, 1000),
        };
    }

}