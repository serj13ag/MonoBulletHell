using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Gameplay.GameObjects;

namespace MonoBulletHell.Gameplay.Services;

public interface IBulletService
{
    void Update();
    void Draw(SpriteBatch spriteBatch);

    void SpawnBullet(Vector2 position, Vector2 direction, float speed);
}

public class BulletService : IBulletService
{
    private readonly ITimeService _timeService;
    private readonly IContentService _contentService;

    private readonly List<Bullet> _bullets = new List<Bullet>(512);

    public BulletService(ITimeService timeService, IContentService contentService)
    {
        _timeService = timeService;
        _contentService = contentService;
    }

    public void Update()
    {
        foreach (var bullet in _bullets)
        {
            bullet.Update();
        }

        // TODO: destroy bullets out of screen
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var bullet in _bullets)
        {
            bullet.Draw(spriteBatch);
        }
    }

    public void SpawnBullet(Vector2 position, Vector2 direction, float speed)
    {
        var bullet = new Bullet(_timeService, _contentService);
        bullet.Position = position;
        bullet.Speed = speed;
        bullet.SetDirection(direction);

        _bullets.Add(bullet);
    }
}