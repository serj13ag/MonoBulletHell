using Microsoft.Xna.Framework;
using MonoBulletHell.Core.Graphics;
using MonoBulletHell.Gameplay.Entities;
using MonoBulletHell.Gameplay.Services;
using MonoBulletHell.Services;

namespace MonoBulletHell.Gameplay.Factories;

public interface IBulletFactory
{
    Bullet CreateBullet(Vector2 position, Vector2 direction, float speed, int damage, bool isPlayer);
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

    public Bullet CreateBullet(Vector2 position, Vector2 direction, float speed, int damage, bool isPlayer)
    {
        var bullet = new Bullet(_debugService, _timeService)
        {
            Position = position,
            Speed = speed,
            Damage = damage,
            IsPlayer = isPlayer,
        };

        bullet.AddSprite(GetBulletSprite(isPlayer), Vector2.Zero);
        bullet.SetDirection(direction);

        return bullet;
    }

    private Sprite GetBulletSprite(bool isPlayer)
    {
        Sprite sprite;
        if (isPlayer)
        {
            sprite = _contentService.CreateBulletSprite("shipBullet");
            sprite.CenterOrigin();
            sprite.Color = Constants.Colors.BeigeColor;
        }
        else
        {
            sprite = _contentService.CreateBulletSprite("enemyBullet");
            sprite.CenterOrigin();
            sprite.Color = Constants.Colors.EnemyColor;
        }

        return sprite;
    }
}