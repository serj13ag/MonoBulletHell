using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Core.Graphics;
using MonoBulletHell.Gameplay.Services;
using MonoBulletHell.Helpers;

namespace MonoBulletHell.Gameplay.GameObjects;

public class Bullet
{
    private const float SpriteBaseRotation = 180f;

    private readonly ITimeService _timeService;

    private readonly Sprite _sprite;

    private Vector2 _direction;

    public Vector2 Position { get; set; }
    public float Speed { get; set; }

    public Bullet(ITimeService timeService, IContentService contentService)
    {
        _timeService = timeService;

        _sprite = contentService.CreateSprite("bullet");
        _sprite.CenterOrigin();
        _sprite.Color = Color.Red;
    }

    public void Update()
    {
        Position += _direction * Speed * _timeService.DeltaTime;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _sprite.Draw(spriteBatch, Position);
    }

    public void SetDirection(Vector2 value)
    {
        _direction = value;
        _sprite.Rotation = GameMathHelper.GetRotation(_direction, SpriteBaseRotation);
    }
}