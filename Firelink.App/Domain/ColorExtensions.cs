
namespace System.Drawing;

public static class ColorExtensions
{

    public static int[] ToRGBAArray(this Color color)
    {
        return [color.R, color.G, color.B, color.A];
    }

    public static Color ShiftBrightness(this Color color, float factor = 0.9f)
    {
        var r = (int)(color.R * factor);
        var g = (int)(color.G * factor);
        var b = (int)(color.B * factor);

        return Color.FromArgb(color.A, Math.Clamp(r, 0, 255), Math.Clamp(g, 0, 255), Math.Clamp(b, 0, 255));
    }

}