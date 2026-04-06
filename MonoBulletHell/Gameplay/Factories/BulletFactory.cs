using Microsoft.Xna.Framework;
using MonoBulletHell.Gameplay.GameObjects;
using MonoBulletHell.Gameplay.Services;
using MonoBulletHell.Services;

namespace MonoBulletHell.Gameplay.Factories;

public interface IBulletFactory
{
    Bullet CreateBullet(Vector2 position, Vector2 direction, float speed, int damage);
}

public class BulletFactory : IBulletFactory
{
    private readonly IDebugService _debugService;
    private readonly ITimeService _timeService;
    private readonly IContentService _contentService;

    public BulletFactory(IDebugService debugService, ITimeService timeService, IContentService contentService)
    {
        _debugService = debugService;
        _timeService = timeService;
        _contentService = contentService;
    }

    public Bullet CreateBullet(Vector2 position, Vector2 direction, float speed, int damage)
    {
        var bullet = new Bullet(_debugService, _timeService, _contentService)
        {
            Position = position,
            Speed = speed,
            Damage = damage,
        };

        bullet.SetDirection(direction);

        return bullet;
    }
}