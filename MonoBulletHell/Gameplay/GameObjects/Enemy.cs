using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Core.Graphics;
using MonoBulletHell.Gameplay.Services;

namespace MonoBulletHell.Gameplay.GameObjects;

public class Enemy
{
    private readonly ITimeService _timeService;
    private readonly IBulletService _bulletService;

    private readonly Sprite _sprite;

    private Vector2 _position;

    public Vector2 Position
    {
        get => _position;
        set => _position = value;
    }

    public Enemy(ITimeService timeService, IContentService contentService, IBulletService bulletService)
    {
        _timeService = timeService;
        _bulletService = bulletService;

        _sprite = GetEnemySprite(contentService);
    }

    public void Update()
    {
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _sprite.Draw(spriteBatch, _position);
    }

    private static Sprite GetEnemySprite(IContentService contentService)
    {
        var sprite = contentService.CreateSprite("enemy");
        sprite.CenterOrigin();
        sprite.Scale = new Vector2(8f, 8f);
        return sprite;
    }
}