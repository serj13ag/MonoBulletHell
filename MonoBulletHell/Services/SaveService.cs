using System;
using System.IO;

namespace MonoBulletHell.Services;

public interface ISaveService
{
    bool TryLoad<T>(string filename, out T data) where T : class;
    void Save<T>(T data, string filename);
}

public class SaveService : ISaveService
{
    private const string GameFolderName = "MonoBulletHell";

    private static readonly string AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    private static readonly string GameDataFolder = Path.Combine(AppDataFolder, GameFolderName);

    private readonly ISerializationService _serializationService;

    public SaveService(ISerializationService serializationService)
    {
        _serializationService = serializationService;
    }

    public bool TryLoad<T>(string filename, out T data) where T : class
    {
        data = null;

        var pathToFile = Path.Combine(GameDataFolder, filename);

        if (!File.Exists(pathToFile))
        {
            return false;
        }

        try
        {
            var file = File.ReadAllText(pathToFile);
            data = _serializationService.DeserializeObject<T>(file);
            return data != null;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error while loading file {pathToFile}, error: {e.Message}");
            return false;
        }
    }

    public void Save<T>(T data, string filename)
    {
        Directory.CreateDirectory(GameDataFolder);

        var pathToFile = Path.Combine(GameDataFolder, filename);
        var json = _serializationService.SerializeObject(data);
        File.WriteAllText(pathToFile, json);
    }
}