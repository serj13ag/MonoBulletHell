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

    private const float MoveSpeed = 400f; // TODO: to config
    private const float BulletSpeed = 1200f; // TODO: to config
    private const int BulletDamage = 1; // TODO: to config

    private readonly Vector2 _bulletSpawnOffset = new Vector2(0f, -32f);

    private readonly IInputService _inputService;
    private readonly IDebugService _debugService;
    private readonly ITimeService _timeService;
    private readonly IBulletService _bulletService;

    private Circle _collider;

    public Circle Collider => _collider;

    public Ship(IInputService inputService, IDebugService debugService, ITimeService timeService, IBulletService bulletService)
    {
        _inputService = inputService;
        _debugService = debugService;
        _timeService = timeService;
        _bulletService = bulletService;
    }

    public void Update()
    {
        if (HasDirectionalInput(out var inputDirection))
        {
            var newPosition = Position + inputDirection * MoveSpeed * _timeService.DeltaTime;
            ScreenHelper.ClampToVirtualBounds(ref newPosition);
            Position = newPosition;
        }

        if (_inputService.Shoot()) // TODO: add cooldown
        {
            _bulletService.SpawnBullet(Position + _bulletSpawnOffset, -Vector2.UnitY, BulletSpeed, BulletDamage, true);
        }

        _collider = new Circle(Position.X, Position.Y, ColliderRadius);
        _debugService.DrawCircle(_collider.Location, _collider.Radius, Color.GreenYellow, 2f, 10);
    }

    public void TakeDamage(int damage)
    {
        // TODO: implement
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