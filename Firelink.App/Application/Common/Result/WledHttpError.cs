using OneOf.Types;

namespace Firelink.Application.Common.Result;

public struct WledHttpError 
{
    public string message;
    public int code;
}


public struct SpotifyError
{
    public string message;
}


public struct NotASpotifyUrl { }