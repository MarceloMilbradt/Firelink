using System.Drawing;

namespace Firelink.Domain;

public class TrackDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public AlbumDto? Album { get; set; }
    public ArtitsDto? Artists { get; set; }
    public string? RGBColor { get; set; }
    public Color Color { get; set; }
}