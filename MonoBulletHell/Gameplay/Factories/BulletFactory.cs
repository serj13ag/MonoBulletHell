using System.Collections.Generic;
using MonoBulletHell.Core.Graphics;
using MonoBulletHell.Data.DTOs;
using MonoBulletHell.Gameplay.Entities;
using MonoBulletHell.Gameplay.Services;
using MonoBulletHell.Services;

namespace MonoBulletHell.Gameplay.Factories;

public interface IBulletFactory
{
    void LoadContent();
    void Prewarm();

    Bullet CreateBullet(in BulletDTO bulletDto);
    void ReleaseBullet(Bullet bullet);
}

public class BulletFactory : IBulletFactory
{
    private const int InitialPoolCapacity = 512;
    private const int PrewarmedBulletCount = 512;

    private readonly IDebugService _debugService;
    private readonly ITimeService _timeService;
    private readonly IContentService _contentService;

    private readonly Stack<Bullet> _bulletPool = new Stack<Bullet>(InitialPoolCapacity);

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
        _playerBulletSprite.Color = Constants.Colors.PlayerBullet;

        _enemyBulletSprite = _contentService.CreateBulletSprite("enemyBullet");
        _enemyBulletSprite.CenterOrigin();
        _enemyBulletSprite.Color = Constants.Colors.EnemyBullet;
    }

    public void Prewarm()
    {
        for (var i = _bulletPool.Count; i < PrewarmedBulletCount; i++)
        {
            _bulletPool.Push(new Bullet(_debugService, _timeService));
        }
    }

    public Bullet CreateBullet(in BulletDTO bulletDto)
    {
        if (_bulletPool.Count == 0)
        {
            return new Bullet(_debugService, _timeService);
        }

        var bullet = _bulletPool.Pop();
        bullet.Init(GetBulletSprite(bulletDto.IsPlayer), in bulletDto);
        return bullet;
    }

    public void ReleaseBullet(Bullet bullet)
    {
        _bulletPool.Push(bullet);
    }

    private Sprite GetBulletSprite(bool isPlayer)
    {
        return isPlayer ? _playerBulletSprite : _enemyBulletSprite;
    }
}