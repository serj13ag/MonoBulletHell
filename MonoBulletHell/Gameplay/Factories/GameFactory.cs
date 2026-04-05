using Microsoft.Xna.Framework;
using MonoBulletHell.GameObjects;
using MonoBulletHell.Services;

namespace MonoBulletHell.Gameplay.Factories;

public class GameFactory
{
    private readonly InputService _inputService;
    private readonly TimeService _timeService;
    private readonly ContentService _contentService;

    public GameFactory(InputService inputService, TimeService timeService, ContentService contentService)
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