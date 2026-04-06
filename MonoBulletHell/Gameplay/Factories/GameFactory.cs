using Microsoft.Xna.Framework;
using MonoBulletHell.Gameplay.GameObjects;
using MonoBulletHell.Gameplay.Services;
using MonoBulletHell.Services;

namespace MonoBulletHell.Gameplay.Factories;

public interface IGameFactory
{
    Ship CreateShip(Vector2 position);
    Enemy CreateEnemy(Vector2 vector2);
}

public class GameFactory : IGameFactory
{
    private readonly IInputService _inputService;
    private readonly IDebugService _debugService;
    private readonly ITimeService _timeService;
    private readonly IContentService _contentService;
    private readonly IBulletService _bulletService;

    public GameFactory(IInputService inputService, IDebugService debugService, ITimeService timeService,
        IContentService contentService, IBulletService bulletService)
    {
        _inputService = inputService;
        _debugService = debugService;
        _timeService = timeService;
        _contentService = contentService;
        _bulletService = bulletService;
    }

    public Ship CreateShip(Vector2 position)
    {
        var ship = new Ship(_inputService, _debugService, _timeService, _contentService, _bulletService);
        ship.Position = position;
        return ship;
    }

    public Enemy CreateEnemy(Vector2 position)
    {
        var enemy = new Enemy(_timeService, _contentService, _bulletService);
        enemy.Position = position;
        return enemy;
    }
}