using System;
using Microsoft.Xna.Framework;
using MonoBulletHell.Core;
using MonoBulletHell.Gameplay.Interfaces;
using MonoBulletHell.Gameplay.Services;
using MonoBulletHell.Services;

namespace MonoBulletHell.Gameplay.Entities;

public class Enemy : EntityWithSprites, IEntityWithCollider
{
    private const int Health = 3; // TODO: to config
    private const float ShootCooldown = 0.5f;
    private const float BulletSpeed = 800;
    private const int BulletDamage = 1;

    private const float ColliderRadius = 45f;

    private readonly IDebugService _debugService;
    private readonly ITimeService _timeService;
    private readonly IBulletService _bulletService;

    private Circle _collider;
    private int _currentHealth = Health;
    private float _timeTillShoot;

    public Circle Collider => _collider;

    public event EventHandler<EventArgs> OnDestroyed;

    public Enemy(IDebugService debugService, ITimeService timeService, IBulletService bulletService)
    {
        _debugService = debugService;
        _timeService = timeService;
        _bulletService = bulletService;
    }

    public void Update()
    {
        HandleShooting();

        _collider = new Circle(Position.X, Position.Y, ColliderRadius);
        _debugService.DrawCircle(_collider.Location, _collider.Radius, Color.GreenYellow, 2f, 10);
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            OnDestroyed?.Invoke(this, EventArgs.Empty);
        }
    }

    private void HandleShooting()
    {
        if (_timeTillShoot <= 0f)
        {
            _bulletService.SpawnBullet(Position, Vector2.UnitY, BulletSpeed, BulletDamage, false);
            _timeTillShoot += ShootCooldown;
        }
        else
        {
            _timeTillShoot -= _timeService.DeltaTime;
        }
    }
}