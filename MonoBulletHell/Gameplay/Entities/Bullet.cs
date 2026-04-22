using Microsoft.Xna.Framework;
using MonoBulletHell.Core;
using MonoBulletHell.Core.Graphics;
using MonoBulletHell.Gameplay.Interfaces;
using MonoBulletHell.Gameplay.Rendering;
using MonoBulletHell.Gameplay.Services;
using MonoBulletHell.Helpers;
using MonoBulletHell.Services;

namespace MonoBulletHell.Gameplay.Entities;

public class Bullet : BaseEntity, IEntityWithCollider
{
    private const float ColliderRadius = 6f;

    private readonly IDebugService _debugService;
    private readonly ITimeService _timeService;

    private readonly Sprite _sprite;

    private Vector2 _direction;
    private Circle _collider;

    public float Speed { get; init; }
    public int Damage { get; init; }
    public bool IsPlayer { get; init; }

    public Circle Collider => _collider;

    public Bullet(IDebugService debugService, ITimeService timeService, Sprite sprite)
    {
        _debugService = debugService;
        _timeService = timeService;

        _sprite = sprite;
    }

    public void Update()
    {
        Position += _direction * Speed * _timeService.DeltaTime;

        _collider = new Circle(Position.X, Position.Y, ColliderRadius);
        _debugService.DrawCircle(_collider.Location, _collider.Radius, Color.GreenYellow, 2f, 10);
    }

    public void SetDirection(Vector2 value)
    {
        _direction = value;
        Rotation = GameMathHelper.GetRotation(_direction);
    }

    public void Render(IRenderService renderService)
    {
        renderService.AddSprite(_sprite, Position, Rotation, Layer.Bullets);
    }
}