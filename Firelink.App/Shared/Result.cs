namespace Firelink.App.Shared;

public record ResultResponse<T>(T Result, bool Success);