using MonoBulletHell.Core.Graphics;
using MonoBulletHell.Data.DTOs;
using MonoBulletHell.Gameplay.Entities;
using MonoBulletHell.Gameplay.Services;
using MonoBulletHell.Services;

namespace MonoBulletHell.Gameplay.Factories;

public interface IBulletFactory
{
    void LoadContent();

    Bullet CreateBullet(in BulletDTO bulletDto);
}

public class BulletFactory : IBulletFactory
{
    private readonly IDebugService _debugService;
    private readonly ITimeService _timeService;
    private readonly IContentService _contentService;

    private Sprite _playerBulletSprite;
    private Sprite _enemyBulletSprite;

    public BulletFactory(IDebugService debugService, ITimeService timeService, IContentService contentService)
    {
        _debugService = debugService;
        _timeService = timeService;
        _contentService = contentService;
    }

    public void LoadContent()
    {
        _playerBulletSprite = _contentService.CreateBulletSprite("shipBullet");
        _playerBulletSprite.CenterOrigin();
        _playerBulletSprite.Color = Constants.Colors.ShipProjectile;

        _enemyBulletSprite = _contentService.CreateBulletSprite("enemyBullet");
        _enemyBulletSprite.CenterOrigin();
        _enemyBulletSprite.Color = Constants.Colors.Orange;
    }

    public Bullet CreateBullet(in BulletDTO bulletDto)
    {
        var bullet = new Bullet(_debugService, _timeService, GetBulletSprite(bulletDto.IsPlayer), in bulletDto);
        return bullet;
    }

    private Sprite GetBulletSprite(bool isPlayer)
    {
        return isPlayer ? _playerBulletSprite : _enemyBulletSprite;
    }
}