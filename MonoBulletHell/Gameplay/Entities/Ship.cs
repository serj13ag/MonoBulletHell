using System;
using Microsoft.Xna.Framework;
using MonoBulletHell.Core;
using MonoBulletHell.Gameplay.Interfaces;
using MonoBulletHell.Gameplay.Services;
using MonoBulletHell.Helpers;
using MonoBulletHell.Services;

namespace MonoBulletHell.Gameplay.Entities;

public class Ship : EntityWithSprites, IEntityWithCollider
{
    private const float ColliderRadius = 10f;

    private const int Health = 3; // TODO: to config
    private const float DamageImmuneCooldown = 2f; // TODO: to config

    private const float ShootCooldown = 0.05f; // TODO: to config
    private const float MoveSpeed = 400f; // TODO: to config

    private const float BulletSpeed = 1200f; // TODO: to config
    private const int BulletDamage = 1; // TODO: to config

    private readonly Vector2 _bulletSpawnOffset = new Vector2(0f, -32f);

    private readonly IInputService _inputService;
    private readonly IDebugService _debugService;
    private readonly ITimeService _timeService;
    private readonly IBulletService _bulletService;

    private Circle _collider;
    private float _timeTillCanShoot;
    private int _currentHealth = Health;
    private bool _isImmune;
    private float _timeTillDisableImmunity;

    public Circle Collider => _collider;
    public bool IsImmune => _isImmune;

    public event Action OnDestroyed;

    public Ship(IInputService inputService, IDebugService debugService, ITimeService timeService, IBulletService bulletService)
    {
        _inputService = inputService;
        _debugService = debugService;
        _timeService = timeService;
        _bulletService = bulletService;
    }

    public void Update()
    {
        var deltaTime = _timeService.DeltaTime;
        HandleMovement(deltaTime);
        HandleShooting(deltaTime);
        HandleImmunity(deltaTime);

        _collider = new Circle(Position.X, Position.Y, ColliderRadius);
        _debugService.DrawCircle(_collider.Location, _collider.Radius, Color.GreenYellow, 2f, 10);
    }

    public void TakeDamage(int damage)
    {
        if (_isImmune)
        {
            return;
        }

        _currentHealth -= damage;

        if (_currentHealth == 0)
        {
            OnDestroyed?.Invoke();
            return;
        }

        EnableImmunity();
    }

    private void HandleMovement(float deltaTime)
    {
        if (HasDirectionalInput(out var inputDirection))
        {
            var newPosition = Position + inputDirection * MoveSpeed * deltaTime;
            ScreenHelper.ClampToVirtualBounds(ref newPosition);
            Position = newPosition;
        }
    }

    private void HandleShooting(float deltaTime)
    {
        if (_timeTillCanShoot > 0f)
        {
            _timeTillCanShoot -= deltaTime;
        }

        if (_inputService.Shoot() && _timeTillCanShoot <= 0f)
        {
            _bulletService.SpawnBullet(Position + _bulletSpawnOffset, -Vector2.UnitY, BulletSpeed, BulletDamage, true);
            _timeTillCanShoot += ShootCooldown;
        }
    }

    private void HandleImmunity(float deltaTime)
    {
        if (!_isImmune)
        {
            return;
        }

        _timeTillDisableImmunity -= deltaTime;
        if (_timeTillDisableImmunity <= 0f)
        {
            DisableImmunity();
        }
    }

    private void EnableImmunity()
    {
        _isImmune = true;
        _timeTillDisableImmunity = DamageImmuneCooldown;
    }

    private void DisableImmunity()
    {
        _isImmune = false;
        _timeTillDisableImmunity = 0f;
    }

    private bool HasDirectionalInput(out Vector2 inputDirection)
    {
        inputDirection = Vector2.Zero;

        if (_inputService.MoveUp())
        {
            inputDirection.Y -= 1f;
        }

        if (_inputService.MoveDown())
        {
            inputDirection.Y += 1f;
        }

        if (_inputService.MoveLeft())
        {
            inputDirection.X -= 1f;
        }

        if (_inputService.MoveRight())
        {
            inputDirection.X += 1f;
        }

        if (inputDirection == Vector2.Zero)
        {
            return false;
        }

        inputDirection.Normalize();
        return true;
    }
}