using Microsoft.Xna.Framework;
using MonoBulletHell.Enums;
using MonoBulletHell.Gameplay.Entities;
using MonoBulletHell.Gameplay.Services;
using MonoBulletHell.Services;

namespace MonoBulletHell.Gameplay.Factories;

public interface IGameFactory
{
    Ship CreateShip();
    Enemy CreateEnemy(Vector2 position, string pathName, EnemyType enemyType);
}

public class GameFactory : IGameFactory
{
    private readonly IInputService _inputService;
    private readonly IDebugService _debugService;
    private readonly ITimeService _timeService;
    private readonly IContentService _contentService;
    private readonly IBulletService _bulletService;
    private readonly IGameContext _gameContext;

    public GameFactory(IInputService inputService, IDebugService debugService, ITimeService timeService,
        IContentService contentService, IBulletService bulletService, IGameContext gameContext)
    {
        _inputService = inputService;
        _debugService = debugService;
        _timeService = timeService;
        _contentService = contentService;
        _bulletService = bulletService;
        _gameContext = gameContext;
    }

    public Ship CreateShip()
    {
        var ship = new Ship(_inputService, _debugService, _timeService, _bulletService, _contentService);
        _gameContext.RegisterShip(ship);
        return ship;
    }

    public Enemy CreateEnemy(Vector2 position, string pathName, EnemyType enemyType)
    {
        var path = _contentService.GetPath(pathName);
        var enemy = new Enemy(_debugService, _timeService, _bulletService, _contentService, position, path, enemyType);
        return enemy;
    }
}