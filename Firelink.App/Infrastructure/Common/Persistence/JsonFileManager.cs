using System.Text.Json;

namespace Firelink.Infrastructure.Common.Persistence;

internal static class JsonFileManager
{
    public static async Task<T> LoadFromJson<T>(string path, CancellationToken cancellation)
    {
        try
        {
            if (File.Exists(path))
            {
                string json = await File.ReadAllTextAsync(path, cancellation);
                return JsonSerializer.Deserialize<T>(json) ?? default!;
            }
            else
            {
                return default!;
            }
        }
        catch (Exception)
        {
            return default!;
        }
    }

    public static async Task SaveToJson<T>(string path, T objectToSave, CancellationToken cancellation)
    {
        string json = JsonSerializer.Serialize(objectToSave);
        await File.WriteAllTextAsync(path, json, cancellation);
    }

}
