using System.Drawing;

namespace Firelink.App.Shared;

public class TrackDto
{
    public string? Name { get; set; }
    public AlbumDto Album { get; set;}
    public ArtitsDto Artists { get; set;}
    public IEnumerable<double> Levels { get; set;}
    public string RGBColor { get; set;}
    public Color Color { get; set; }
   
}