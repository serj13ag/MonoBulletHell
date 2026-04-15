using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Core.Graphics;
using MonoBulletHell.Data;
using Newtonsoft.Json;

namespace MonoBulletHell.Gameplay.Services;

public interface IContentService
{
    void Load(ContentManager content);

    Sprite CreateSprite(string spriteName);
    Effect GetFlashEffect();
    SpawnData GetSpawnData();
    Texture2D BackgroundTexture { get; }
}

public class ContentService : IContentService
{
    private TextureAtlas _atlas;
    private Effect _flashEffect;
    private SpawnData _spawnData;

    public Texture2D BackgroundTexture { get; private set; }

    public void Load(ContentManager content)
    {
        _atlas = TextureAtlas.FromFile(content, "images/atlas-definition.json");
        BackgroundTexture = content.Load<Texture2D>("images/background");
        _flashEffect = content.Load<Effect>("shaders/flashEffect");
        _spawnData = LoadJsonData<SpawnData>(content, "configs/spawnData.json");
    }

    public Sprite CreateSprite(string spriteName)
    {
        return _atlas.CreateSprite(spriteName);
    }

    public Effect GetFlashEffect()
    {
        return _flashEffect;
    }

    public SpawnData GetSpawnData()
    {
        return _spawnData;
    }

    private static T LoadJsonData<T>(ContentManager content, string fileName)
    {
        var filePath = Path.Combine(content.RootDirectory, fileName);
        var json = File.ReadAllText(filePath);

        var spawnData = JsonConvert.DeserializeObject<T>(json);

        return spawnData;
    }
}