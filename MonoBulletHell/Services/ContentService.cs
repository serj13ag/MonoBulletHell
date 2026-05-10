using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Core.Graphics;
using MonoBulletHell.Core.Graphics.Data;
using MonoBulletHell.Data.Configs;
using MonoBulletHell.Gameplay.Effects;

namespace MonoBulletHell.Services;

public interface IContentService
{
    void Load(ContentManager content);

    Sprite CreateShipSprite(string spriteName);
    Sprite CreateBulletSprite(string spriteName);
    AnimatedSprite CreateBulletAnimatedSprite(string animationName);

    FlashEffect GetFlashEffect();

    PlayerConfig GetPlayerConfig();
    ColorConfig GetColorConfig();
    LevelConfig GetLevelConfig();
    EnemyData GetEnemyData(string enemyName);
    PathData GetPath(string pathName);
    EmitterData GetEmitterData(string emitterName);

    Texture2D BackgroundTexture { get; }
}

public class ContentService : IContentService
{
    private readonly ISerializationService _serializationService;

    private TextureAtlas _shipsAtlas;
    private TextureAtlas _bulletsAtlas;

    private Effect _flashEffect;

    private GameConfig _gameConfig;
    private LevelConfig _levelConfig;
    private Dictionary<string, EnemyData> _enemyConfigs;
    private Dictionary<string, PathData> _pathConfigs;
    private Dictionary<string, EmitterData> _emitterConfigs;

    public Texture2D BackgroundTexture { get; private set; }

    public ContentService(ISerializationService serializationService)
    {
        _serializationService = serializationService;
    }

    public void Load(ContentManager content)
    {
        var shipsAtlasData = LoadJsonData<TextureAtlasData>(content, "images/ships-atlas.json");
        _shipsAtlas = TextureAtlas.FromFile(content, shipsAtlasData);
        var bulletsAtlasData = LoadJsonData<TextureAtlasData>(content, "images/bullets-atlas.json");
        _bulletsAtlas = TextureAtlas.FromFile(content, bulletsAtlasData);

        BackgroundTexture = content.Load<Texture2D>("images/background");
        _flashEffect = content.Load<Effect>("shaders/flashEffect");

        _gameConfig = LoadJsonData<GameConfig>(content, "configs/gameConfig.json");
        _levelConfig = LoadJsonData<LevelConfig>(content, "configs/levelConfig.json");

        var enemies = LoadJsonData<List<EnemyData>>(content, "configs/enemies.json");
        _enemyConfigs = enemies.ToDictionary(x => x.Name);

        var paths = LoadJsonData<List<PathData>>(content, "configs/paths.json");
        _pathConfigs = paths.ToDictionary(path => path.Name);

        var emitters = LoadJsonData<List<EmitterData>>(content, "configs/emitters.json");
        _emitterConfigs = emitters.ToDictionary(x => x.Name);

        ValidateData();
    }

    public Sprite CreateShipSprite(string spriteName) => _shipsAtlas.CreateSprite(spriteName);
    public Sprite CreateBulletSprite(string spriteName) => _bulletsAtlas.CreateSprite(spriteName);
    public AnimatedSprite CreateBulletAnimatedSprite(string animationName) => _bulletsAtlas.CreateAnimatedSprite(animationName);

    public FlashEffect GetFlashEffect() => new FlashEffect(_flashEffect.Clone());

    public PlayerConfig GetPlayerConfig() => _gameConfig.Player;
    public ColorConfig GetColorConfig() => _gameConfig.Colors;

    public LevelConfig GetLevelConfig() => _levelConfig;

    public EnemyData GetEnemyData(string enemyName) => _enemyConfigs[enemyName];
    public PathData GetPath(string pathName) => _pathConfigs[pathName];
    public EmitterData GetEmitterData(string emitterName) => _emitterConfigs[emitterName];

    private T LoadJsonData<T>(ContentManager content, string fileName)
    {
        var filePath = Path.Combine(content.RootDirectory, fileName);
        var json = File.ReadAllText(filePath);
        return _serializationService.DeserializeObject<T>(json);
    }

    private void ValidateData()
    {
        ValidatePaths();
        ValidateLevel();
        ValidateEmitters();
        ValidateEnemies();
    }

    private void ValidatePaths()
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

            var isUsed = _levelConfig.Waves.Any(x => x.PathName == path.Name) ||
                         _levelConfig.Boss?.Stages.Any(x => x.PathName == path.Name) == true;
            if (!isUsed)
            {
                Console.WriteLine($"Warning: Unused path: {path.Name}");
            }
        }
    }

    private void ValidateLevel()
    {
        foreach (var waveData in _levelConfig.Waves)
        {
            if (!_pathConfigs.ContainsKey(waveData.PathName))
            {
                throw new Exception("Path not found: " + waveData.PathName);
            }
        }

        if (_levelConfig.Boss != null)
        {
            if (_levelConfig.Boss.Stages == null || _levelConfig.Boss.Stages.Count == 0)
            {
                throw new Exception("Boss must have at least 1 stage");
            }
        }
    }

    private void ValidateEmitters()
    {
        foreach (var emitter in _emitterConfigs.Values)
        {
            var isUsed = _levelConfig.Waves.Any(x => x.EmitterName == emitter.Name) ||
                         _levelConfig.Boss?.Stages.Any(x => x.EmitterName == emitter.Name) == true;
            if (!isUsed)
            {
                Console.WriteLine($"Warning: Unused emitter: {emitter.Name}");
            }
        }
    }

    private void ValidateEnemies()
    {
        foreach (var enemy in _enemyConfigs.Values)
        {
            var isUsed = _levelConfig.Waves.Any(x => x.EnemyName == enemy.Name) ||
                         _levelConfig.Boss?.EnemyName == enemy.Name;
            if (!isUsed)
            {
                Console.WriteLine($"Warning: Unused enemy: {enemy.Name}");
            }
        }
    }
}