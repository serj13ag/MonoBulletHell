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

    Sprite CreateShipSprite(string spriteName);
    Sprite CreateBulletSprite(string spriteName);
    AnimatedSprite CreateBulletAnimatedSprite(string animationName);

    Effect GetFlashEffect();

    PlayerConfig GetPlayerConfig();
    SpawnConfig GetSpawnConfig();
    EnemyData GetEnemyData(string enemyName);
    PathData GetPath(string pathName);

    Texture2D BackgroundTexture { get; }
}

public class ContentService : IContentService
{
    private TextureAtlas _shipsAtlas;
    private TextureAtlas _bulletsAtlas;

    private Effect _flashEffect;

    private PlayerConfig _playerConfig;
    private SpawnConfig _spawnConfig;
    private Dictionary<string, EnemyData> _enemyConfigs;
    private Dictionary<string, PathData> _pathConfigs;

    public Texture2D BackgroundTexture { get; private set; }

    public void Load(ContentManager content)
    {
        _shipsAtlas = TextureAtlas.FromFile(content, "images/ships-atlas.json");
        _bulletsAtlas = TextureAtlas.FromFile(content, "images/bullets-atlas.json");

        BackgroundTexture = content.Load<Texture2D>("images/background");
        _flashEffect = content.Load<Effect>("shaders/flashEffect");

        _playerConfig = LoadJsonData<PlayerConfig>(content, "configs/playerConfig.json");
        _spawnConfig = LoadJsonData<SpawnConfig>(content, "configs/spawnConfig.json");

        var enemyConfig = LoadJsonData<EnemyConfig>(content, "configs/enemyConfig.json");
        _enemyConfigs = enemyConfig.Enemies.ToDictionary(x => x.Name);

        var pathConfig = LoadJsonData<PathConfig>(content, "configs/pathConfig.json");
        _pathConfigs = pathConfig.Paths.ToDictionary(path => path.Name);

        ValidateData();
    }

    public Sprite CreateShipSprite(string spriteName) => _shipsAtlas.CreateSprite(spriteName);
    public Sprite CreateBulletSprite(string spriteName) => _bulletsAtlas.CreateSprite(spriteName);
    public AnimatedSprite CreateBulletAnimatedSprite(string animationName) => _bulletsAtlas.CreateAnimatedSprite(animationName);

    public Effect GetFlashEffect() => _flashEffect.Clone();

    public PlayerConfig GetPlayerConfig() => _playerConfig;
    public SpawnConfig GetSpawnConfig() => _spawnConfig;

    public EnemyData GetEnemyData(string enemyName) => _enemyConfigs[enemyName];
    public PathData GetPath(string pathName) => _pathConfigs[pathName];

    private static T LoadJsonData<T>(ContentManager content, string fileName)
    {
        var filePath = Path.Combine(content.RootDirectory, fileName);
        var json = File.ReadAllText(filePath);

        var spawnData = JsonConvert.DeserializeObject<T>(json);

        return spawnData;
    }

    private void ValidateData()
    {
        foreach (var path in _pathConfigs.Values)
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

        foreach (var waveData in _spawnConfig.Waves)
        {
            if (!_pathConfigs.ContainsKey(waveData.PathName))
            {
                throw new Exception("Path not found: " + waveData.PathName);
            }
        }
    }
}