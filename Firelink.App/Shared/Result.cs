namespace Firelink.App.Shared;

public record ResultResponse<T>(T Result, bool Success)
{
    public static ResultResponse<T> Ok(T result)
    {
        return new ResultResponse<T>(result, true);
    }
    public static ResultResponse<T> BadRequest(T result)
    {
        return new ResultResponse<T>(result, false);
    }
}
