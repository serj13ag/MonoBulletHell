using Microsoft.Xna.Framework;
using MonoBulletHell.Gameplay.Entities;
using MonoBulletHell.Gameplay.Entities.Emitters;
using MonoBulletHell.Gameplay.Services;
using MonoBulletHell.Services;

namespace MonoBulletHell.Gameplay.Factories;

public interface IGameFactory
{
    Ship CreateShip();
    Enemy CreateEnemy(Vector2 position, string pathName, string enemyName, string emitterName);
    IBulletEmitter CreateEmitter(string emitterName);
}

public class GameFactory : IGameFactory
{
    private readonly IInputActionService _inputActionService;
    private readonly IDebugService _debugService;
    private readonly ITimeService _timeService;
    private readonly IContentService _contentService;
    private readonly IBulletService _bulletService;
    private readonly IGameContext _gameContext;
    private readonly ISoundService _soundService;

    public GameFactory(IInputActionService inputActionService, IDebugService debugService, ITimeService timeService,
        IContentService contentService, IBulletService bulletService, IGameContext gameContext, ISoundService soundService)
    {
        _inputActionService = inputActionService;
        _debugService = debugService;
        _timeService = timeService;
        _contentService = contentService;
        _bulletService = bulletService;
        _gameContext = gameContext;
        _soundService = soundService;
    }

    public Ship CreateShip()
    {
        var playerConfig = _contentService.GetPlayerConfig();
        var ship = new Ship(_inputActionService, _debugService, _timeService, _bulletService, _contentService, _soundService,
            playerConfig);
        _gameContext.RegisterShip(ship);
        return ship;
    }

    public Enemy CreateEnemy(Vector2 position, string pathName, string enemyName, string emitterName)
    {
        var path = _contentService.GetPath(pathName);
        var enemyData = _contentService.GetEnemyData(enemyName);
        var bulletEmitter = CreateEmitter(emitterName);
        var enemy = new Enemy(_debugService, _timeService, _contentService, _soundService, position, path, enemyData,
            bulletEmitter);
        return enemy;
    }

    public IBulletEmitter CreateEmitter(string emitterName)
    {
        if (string.IsNullOrEmpty(emitterName))
        {
            return NullBulletEmitter.Instance;
        }

        var emitterData = _contentService.GetEmitterData(emitterName);
        return new BulletEmitter(emitterData, _bulletService);
    }
}