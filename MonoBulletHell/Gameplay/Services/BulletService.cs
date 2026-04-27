using System.Collections.Generic;
using MonoBulletHell.Core.Physics;
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

    void SpawnBullet(BulletDTO bulletDto);
    void Clear();
}

public class BulletService : IBulletService
{
    private readonly IGameContext _gameContext;
    private readonly IBulletFactory _bulletFactory;
    private readonly IParticleService _particleService;
    private readonly IDebugService _debugService;

    private readonly List<Bullet> _bullets = new List<Bullet>(512);
    private readonly List<Bullet> _bulletsToDestroy = new List<Bullet>(128);

    public BulletService(IGameContext gameContext, IBulletFactory bulletFactory, IParticleService particleService,
        IDebugService debugService)
    {
        _gameContext = gameContext;
        _bulletFactory = bulletFactory;
        _particleService = particleService;
        _debugService = debugService;
    }

    public void Update()
    {
        foreach (var bullet in _bullets)
        {
            bullet.Update();

            if (ScreenHelper.IsOutOfVirtualBounds(bullet.Position))
            {
                MarkForDestroy(bullet);
                continue;
            }

            if (bullet.IsPlayer)
            {
                HandlePlayerBulletCollision(bullet);
            }
            else
            {
                HandleEnemyBulletCollision(bullet);
            }
        }

        RemoveDestroyedBullets();

        _debugService.ShowBulletCount(_bullets.Count);
    }

    public void Render(IRenderService renderService)
    {
        foreach (var bullet in _bullets)
        {
            bullet.Render(renderService);
        }
    }

    public void SpawnBullet(BulletDTO bulletDto)
    {
        var bullet = _bulletFactory.CreateBullet(bulletDto);
        _bullets.Add(bullet);
    }

    public void Clear()
    {
        _bullets.Clear();
    }

    private void HandlePlayerBulletCollision(Bullet bullet)
    {
        foreach (var enemy in _gameContext.Enemies)
        {
            if (!IsColliding(bullet, enemy))
            {
                continue;
            }

            enemy.TakeDamage(bullet.Damage);
            _particleService.CreateBulletImpact(bullet.Position);
            MarkForDestroy(bullet);
            return;
        }
    }

    private void HandleEnemyBulletCollision(Bullet bullet)
    {
        var ship = _gameContext.Ship;
        if (ship.IsImmune || !IsColliding(bullet, ship))
        {
            return;
        }

        ship.TakeDamage(bullet.Damage);
        _particleService.CreateBulletImpact(bullet.Position);
        MarkForDestroy(bullet);
    }

    private void MarkForDestroy(Bullet bullet)
    {
        _bulletsToDestroy.Add(bullet);
    }

    private void RemoveDestroyedBullets()
    {
        foreach (var bulletToDestroy in _bulletsToDestroy)
        {
            _bullets.Remove(bulletToDestroy);
        }

        _bulletsToDestroy.Clear();
    }

    private static bool IsColliding(Bullet bullet, IEntityWithCollider entityWithCollider)
    {
        return bullet.Collider.Intersects(entityWithCollider.Collider);
    }
}