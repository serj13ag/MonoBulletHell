using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Core;
using MonoBulletHell.Core.Graphics;
using MonoBulletHell.Gameplay.Services;
using MonoBulletHell.Helpers;
using MonoBulletHell.Services;

namespace MonoBulletHell.Gameplay.GameObjects;

public class Bullet
{
    private const float SpriteBaseRotation = 180f;
    private const float ColliderRadius = 6f;

    private readonly IDebugService _debugService;
    private readonly ITimeService _timeService;

    private readonly Sprite _sprite;

    private Vector2 _direction;
    private Vector2 _position;
    private Circle _collider;

    public Vector2 Position
    {
        get => _position;
        init => _position = value;
    }

    public float Speed { get; init; }
    public int Damage { get; init; }

    public Circle Collider => _collider;

    public Bullet(IDebugService debugService, ITimeService timeService, IContentService contentService)
    {
        _debugService = debugService;
        _timeService = timeService;

        _sprite = contentService.CreateSprite("bullet");
        _sprite.CenterOrigin();
        _sprite.Color = Color.Red;
    }

    public void Update()
    {
        _position += _direction * Speed * _timeService.DeltaTime;

        _collider = new Circle(_position.X, _position.Y, ColliderRadius);
        _debugService.DrawCircle(_collider.Location, _collider.Radius, Color.GreenYellow, 2f, 10);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _sprite.Draw(spriteBatch, _position);
    }

    public void SetDirection(Vector2 value)
    {
        _direction = value;
        _sprite.Rotation = GameMathHelper.GetRotation(_direction, SpriteBaseRotation);
    }
}