namespace Firelink.Infrastructure.Common.Colors;

public struct HSV
{
    public int h;
    public int s;
    public int v;
    public override string ToString()
    {
        return h.ToString("X4").ToLower() + s.ToString("X4").ToLower() + v.ToString("X4").ToLower();
    }
}