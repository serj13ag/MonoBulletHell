using Microsoft.Xna.Framework;
using MonoBulletHell.Core.Graphics;
using MonoBulletHell.Gameplay.Entities;
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
    private const float ShipSpriteScale = 4f;
    private const float CoreSpriteScale = 2f;

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

    public Ship CreateShip(Vector2 position)
    {
        var ship = new Ship(_inputService, _debugService, _timeService, _bulletService)
        {
            Position = position,
        };

        ship.AddSprite(GetShipSprite(_contentService), Vector2.Zero);
        ship.AddSprite(GetCoreSprite(_contentService), Vector2.Zero);

        _gameContext.RegisterShip(ship);
        return ship;
    }

    public Enemy CreateEnemy(Vector2 position)
    {
        var enemy = new Enemy(_debugService, _timeService, _bulletService)
        {
            Position = position,
        };

        var sprite = _contentService.CreateSprite("enemy");
        sprite.CenterOrigin();
        sprite.Scale = new Vector2(8f, 8f);
        enemy.AddSprite(sprite, new Vector2(0f, -25f));

        _gameContext.RegisterEnemy(enemy);
        return enemy;
    }

    private static Sprite GetShipSprite(IContentService contentService)
    {
        var sprite = contentService.CreateSprite("ship");
        sprite.CenterOrigin();
        sprite.Scale = new Vector2(ShipSpriteScale, ShipSpriteScale);
        return sprite;
    }

    private static Sprite GetCoreSprite(IContentService contentService)
    {
        var sprite = contentService.CreateSprite("shipCore");
        sprite.CenterOrigin();
        sprite.Color = Color.Red;
        sprite.Scale = new Vector2(CoreSpriteScale, CoreSpriteScale);
        return sprite;
    }
}