using System.Drawing;

namespace Firelink.Application.Common.Interfaces;

public interface IAlbumColorProvider
{
    public Task<Color> GetColorForAlbumUrl(string albumUrl);
}
