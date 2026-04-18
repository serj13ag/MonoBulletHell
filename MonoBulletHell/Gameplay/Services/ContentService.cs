using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    Sprite CreateBulletSprite(string spriteName);
    AnimatedSprite CreateBulletAnimatedSprite(string animationName);

    Effect GetFlashEffect();
    SpawnData GetSpawnData();
    PathData GetPath(string pathName);

    Texture2D BackgroundTexture { get; }
}

public class ContentService : IContentService
{
    private TextureAtlas _atlas;
    private TextureAtlas _bulletsAtlas;

    private Effect _flashEffect;

    private SpawnData _spawnData;
    private Dictionary<string, PathData> _paths;

    public Texture2D BackgroundTexture { get; private set; }

    public void Load(ContentManager content)
    {
        _atlas = TextureAtlas.FromFile(content, "images/atlas-definition.json");
        _bulletsAtlas = TextureAtlas.FromFile(content, "images/bullets-atlas.json");

        BackgroundTexture = content.Load<Texture2D>("images/background");
        _flashEffect = content.Load<Effect>("shaders/flashEffect");

        _spawnData = LoadJsonData<SpawnData>(content, "configs/spawnData.json");
        _paths = LoadPaths(content);

        ValidateData();
    }

    public Sprite CreateSprite(string spriteName)
    {
        return _atlas.CreateSprite(spriteName);
    }

    public Sprite CreateBulletSprite(string spriteName)
    {
        return _bulletsAtlas.CreateSprite(spriteName);
    }

    public AnimatedSprite CreateBulletAnimatedSprite(string animationName)
    {
        return _bulletsAtlas.CreateAnimatedSprite(animationName);
    }

    public Effect GetFlashEffect()
    {
        return _flashEffect.Clone();
    }

    public SpawnData GetSpawnData()
    {
        return _spawnData;
    }

    public PathData GetPath(string pathName)
    {
        return _paths[pathName];
    }

    private static Dictionary<string, PathData> LoadPaths(ContentManager content)
    {
        var pathConfig = LoadJsonData<PathConfig>(content, "configs/pathConfig.json");
        return pathConfig.Paths.ToDictionary(path => path.Name);
    }

    private static T LoadJsonData<T>(ContentManager content, string fileName)
    {
        var filePath = Path.Combine(content.RootDirectory, fileName);
        var json = File.ReadAllText(filePath);

        var spawnData = JsonConvert.DeserializeObject<T>(json);

        return spawnData;
    }

    private void ValidateData()
    {
        foreach (var path in _paths.Values)
        {
            if (path.Points.Count < 2)
            {
                throw new Exception("Path must have at least 2 points");
            }

            foreach (var pathPointData in path.Points)
            {
                if (pathPointData.ControlPoints?.Count > 2)
                {
                    throw new Exception("Path must have max 2 control points");
                }
            }
        }

        foreach (var waveData in _spawnData.Waves)
        {
            if (!_paths.ContainsKey(waveData.PathName))
            {
                throw new Exception("Path not found: " + waveData.PathName);
            }
        }
    }
}