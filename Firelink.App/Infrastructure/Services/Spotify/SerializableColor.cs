using MemoryPack;
using System.Drawing;

namespace Firelink.Infrastructure.Services.Spotify;

[MemoryPackable]
public partial class SerializableColor
{
    // Public properties to store ARGB values
    public byte A { get; set; }
    public byte R { get; set; }
    public byte G { get; set; }
    public byte B { get; set; }

    // Implicit conversion from System.Drawing.Color to SerializableColor
    public static implicit operator SerializableColor(Color color)
    {
        return new SerializableColor
        {
            A = color.A,
            R = color.R,
            G = color.G,
            B = color.B
        };
    }

    // Implicit conversion from SerializableColor to System.Drawing.Color
    public static implicit operator Color(SerializableColor serializableColor)
    {
        return Color.FromArgb(
            serializableColor.A,
            serializableColor.R,
            serializableColor.G,
            serializableColor.B
        );
    }
}