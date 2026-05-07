using Microsoft.Xna.Framework;
using MonoBulletHell.Core.Graphics;
using MonoBulletHell.Core.Physics;
using MonoBulletHell.Data.DTOs;
using MonoBulletHell.Gameplay.Rendering;
using MonoBulletHell.Gameplay.Services;
using MonoBulletHell.Helpers;
using MonoBulletHell.Services;

namespace MonoBulletHell.Gameplay.Entities;

public class Bullet : BaseEntity
{
    private const float ColliderRadius = 6f;

    private readonly IDebugService _debugService;
    private readonly ITimeService _timeService;

    private readonly CircleCollider _collider;

    private Sprite _sprite;

    private float _acceleration;
    private float _angularVelocity;

    private float _speed;
    private Vector2 _direction;

    public int Damage => 1;
    public bool IsPlayer { get; private set; }

    public CircleCollider Collider => _collider;

    public Bullet(IDebugService debugService, ITimeService timeService)
    {
        _debugService = debugService;
        _timeService = timeService;

        _collider = new CircleCollider(Vector2.Zero, ColliderRadius);
    }

    public void Init(Sprite sprite, in BulletDTO bulletDto)
    {
        _sprite = sprite;

        Position = bulletDto.Position;
        _speed = bulletDto.Speed;
        IsPlayer = bulletDto.IsPlayer;
        _acceleration = bulletDto.Acceleration;
        _angularVelocity = bulletDto.AngularVelocity;
        SetDirection(bulletDto.Direction);

        _collider.Update(Position);
    }

    public void Update()
    {
        var deltaTime = _timeService.DeltaTime;

        _speed += _acceleration * deltaTime;
        _direction.Rotate(-MathHelper.ToRadians(_angularVelocity * deltaTime));

        Position += _direction * _speed * deltaTime;

        _collider.Update(Position);
        _debugService.DrawCircle(_collider.Center, _collider.Radius, Color.GreenYellow, 2f, 10);
    }

    public void Render(IRenderService renderService)
    {
        renderService.AddSprite(_sprite, Position, Rotation, Layer.Bullets);
    }

    private void SetDirection(Vector2 value)
    {
        _direction = value;
        Rotation = GameMathHelper.GetRotation(_direction);
    }
}