using System.Collections.Generic;
using MonoBulletHell.Data.DTOs;
using MonoBulletHell.Gameplay.Entities;
using MonoBulletHell.Gameplay.Factories;
using MonoBulletHell.Helpers;
using MonoBulletHell.Services;

namespace MonoBulletHell.Gameplay.Services;

public interface IBulletService
{
    void Update();
    void Render(IRenderService renderService);

    void SpawnBullet(in BulletDTO bulletDto);
    void Clear();
}

public class BulletService : IBulletService
{
    private readonly IGameContext _gameContext;
    private readonly IBulletFactory _bulletFactory;
    private readonly IParticleService _particleService;
    private readonly IDebugService _debugService;
    private readonly ISettingsService _settingsService;

    private readonly List<Bullet> _bullets = new List<Bullet>(512);

    public BulletService(IGameContext gameContext, IBulletFactory bulletFactory, IParticleService particleService,
        IDebugService debugService, ISettingsService settingsService)
    {
        _gameContext = gameContext;
        _bulletFactory = bulletFactory;
        _particleService = particleService;
        _debugService = debugService;
        _settingsService = settingsService;
    }

    public void Update()
    {
        for (var i = _bullets.Count - 1; i >= 0; i--)
        {
            var bullet = _bullets[i];
            bullet.Update();

            if (ScreenHelper.IsOutOfVirtualBounds(bullet.Position))
            {
                RemoveBulletAt(i);
                continue;
            }

            var shouldDestroy = bullet.IsPlayer
                ? HandlePlayerBulletCollision(bullet)
                : HandleEnemyBulletCollision(bullet);

            if (shouldDestroy)
            {
                RemoveBulletAt(i);
            }
        }

        _debugService.ShowBulletCount(_bullets.Count);
    }

    public void Render(IRenderService renderService)
    {
        foreach (var bullet in _bullets)
        {
            bullet.Render(renderService);
        }
    }

    public void SpawnBullet(in BulletDTO bulletDto)
    {
        var bullet = _bulletFactory.CreateBullet(in bulletDto);
        _bullets.Add(bullet);
    }

    public void Clear()
    {
        foreach (var bullet in _bullets)
        {
            _bulletFactory.ReleaseBullet(bullet);
        }

        _bullets.Clear();
    }

    private void RemoveBulletAt(int index)
    {
        var bullet = _bullets[index];
        var lastIndex = _bullets.Count - 1;

        _bullets[index] = _bullets[lastIndex];
        _bullets.RemoveAt(lastIndex);

        _bulletFactory.ReleaseBullet(bullet);
    }

    private bool HandlePlayerBulletCollision(Bullet bullet)
    {
        foreach (var enemy in _gameContext.Enemies)
        {
            if (!bullet.Collider.Intersects(enemy.Collider))
            {
                continue;
            }

            enemy.TakeDamage(bullet.Damage);
            _particleService.CreateBulletImpact(bullet.Position);
            return true;
        }

        return false;
    }

    private bool HandleEnemyBulletCollision(Bullet bullet)
    {
        if (_settingsService.GodModeEnabled)
        {
            return false;
        }

        var ship = _gameContext.Ship;
        if (ship.IsImmune || !bullet.Collider.Intersects(ship.Collider))
        {
            return false;
        }

        ship.TakeDamage(bullet.Damage);
        _particleService.CreateBulletImpact(bullet.Position);
        return true;
    }
}