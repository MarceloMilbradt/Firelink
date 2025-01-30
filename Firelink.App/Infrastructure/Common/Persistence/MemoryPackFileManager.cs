using MemoryPack;

namespace Firelink.Infrastructure.Common.Persistence;

internal static class MemoryPackFileManager
{
    public static async Task<T> LoadFromFile<T>(string path, CancellationToken cancellation)
    {
        try
        {
            string applicationPath = GetFileRoot();
            var fullPath = Path.Combine(applicationPath, path);
            if (File.Exists(fullPath))
            {
                byte[] content = await File.ReadAllBytesAsync(fullPath, cancellation);
                return MemoryPackSerializer.Deserialize<T>(content) ?? default!;
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

    private static string GetFileRoot()
    {
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var applicationPath = Path.Combine(appDataPath, "firelink");
        Directory.CreateDirectory(applicationPath);
        return applicationPath;
    }

    public static async Task SaveToFile<T>(string path, T objectToSave, CancellationToken cancellation)
    {
        var filePath = Path.Combine(GetFileRoot(), path);
        byte[] content = MemoryPackSerializer.Serialize(objectToSave);
        await File.WriteAllBytesAsync(filePath, content, cancellation);
    }

}
