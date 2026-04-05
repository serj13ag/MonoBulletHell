using Microsoft.Xna.Framework;
using MonoBulletHell.Gameplay.GameObjects;
using MonoBulletHell.Gameplay.Services;
using MonoBulletHell.Services;

namespace MonoBulletHell.Gameplay.Factories;

public interface IGameFactory
{
    Ship CreateShip(Vector2 position);
}

public class GameFactory : IGameFactory
{
    private readonly IInputService _inputService;
    private readonly ITimeService _timeService;
    private readonly IContentService _contentService;

    public GameFactory(IInputService inputService, ITimeService timeService, IContentService contentService)
    {
        _inputService = inputService;
        _timeService = timeService;
        _contentService = contentService;
    }

    public Ship CreateShip(Vector2 position)
    {
        var ship = new Ship(_inputService, _timeService, _contentService);
        ship.Position = position;
        return ship;
    }
}