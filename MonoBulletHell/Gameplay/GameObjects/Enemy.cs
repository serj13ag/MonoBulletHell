using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Core;
using MonoBulletHell.Core.Graphics;
using MonoBulletHell.Gameplay.Services;
using MonoBulletHell.Services;

namespace MonoBulletHell.Gameplay.GameObjects;

public class Enemy
{
    private const float ColliderRadius = 45f;
    private const float SpriteScale = 8f;
    private static readonly Vector2 SpriteOffset = new Vector2(0f, -25f);

    private readonly IDebugService _debugService;
    private readonly ITimeService _timeService;
    private readonly IBulletService _bulletService;

    private readonly Sprite _sprite;

    private Vector2 _position;

    public Vector2 Position
    {
        get => _position;
        set => _position = value;
    }

    public Enemy(IDebugService debugService, ITimeService timeService, IContentService contentService,
        IBulletService bulletService)
    {
        _debugService = debugService;
        _timeService = timeService;
        _bulletService = bulletService;

        _sprite = GetEnemySprite(contentService);
    }

    public void Update()
    {
        var enemyBounds = new Circle(_position.X, _position.Y, ColliderRadius);
        _debugService.DrawCircle(enemyBounds.Location, enemyBounds.Radius, Color.GreenYellow, 2f, 10);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _sprite.Draw(spriteBatch, _position + SpriteOffset);
    }

    private static Sprite GetEnemySprite(IContentService contentService)
    {
        var sprite = contentService.CreateSprite("enemy");
        sprite.CenterOrigin();
        sprite.Scale = new Vector2(SpriteScale, SpriteScale);
        return sprite;
    }
}