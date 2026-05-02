using MonoBulletHell.Core.Graphics;
using MonoBulletHell.Data.DTOs;
using MonoBulletHell.Gameplay.Entities;
using MonoBulletHell.Gameplay.Services;
using MonoBulletHell.Services;

namespace MonoBulletHell.Gameplay.Factories;

public interface IBulletFactory
{
    Bullet CreateBullet(in BulletDTO bulletDto);
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

    public Bullet CreateBullet(in BulletDTO bulletDto)
    {
        var bullet = new Bullet(_debugService, _timeService, GetBulletSprite(bulletDto.IsPlayer), in bulletDto);
        return bullet;
    }

    private Sprite GetBulletSprite(bool isPlayer)
    {
        Sprite sprite;
        if (isPlayer)
        {
            sprite = _contentService.CreateBulletSprite("shipBullet");
            sprite.CenterOrigin();
            sprite.Color = Constants.Colors.ShipProjectile;
        }
        else
        {
            sprite = _contentService.CreateBulletSprite("enemyBullet");
            sprite.CenterOrigin();
            sprite.Color = Constants.Colors.Orange;
        }

        return sprite;
    }
}