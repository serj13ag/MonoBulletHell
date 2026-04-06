using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Gameplay.Factories;
using MonoBulletHell.Gameplay.GameObjects;
using MonoBulletHell.Helpers;

namespace MonoBulletHell.Gameplay.Services;

public interface IBulletService
{
    void Update();
    void Draw(SpriteBatch spriteBatch);

    void SpawnBullet(Vector2 position, Vector2 direction, float speed, int damage);
}

public class BulletService : IBulletService
{
    private readonly IGameContext _gameContext;
    private readonly IBulletFactory _bulletFactory;

    private readonly List<Bullet> _bullets = new List<Bullet>(512);
    private readonly List<Bullet> _bulletsToDestroy = new List<Bullet>(128);

    public BulletService(IGameContext gameContext, IBulletFactory bulletFactory)
    {
        _gameContext = gameContext;
        _bulletFactory = bulletFactory;
    }

    public void Update()
    {
        foreach (var bullet in _bullets)
        {
            bullet.Update();

            if (IsColliding(bullet, _gameContext.Enemy))
            {
                _gameContext.Enemy.TakeDamage(bullet.Damage);
                _bulletsToDestroy.Add(bullet);
            }
            else if (ScreenHelper.IsOutOfVirtualBounds(bullet.Position))
            {
                _bulletsToDestroy.Add(bullet);
            }
        }

        foreach (var bulletToDestroy in _bulletsToDestroy)
        {
            _bullets.Remove(bulletToDestroy);
        }

        _bulletsToDestroy.Clear();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var bullet in _bullets)
        {
            bullet.Draw(spriteBatch);
        }
    }

    public void SpawnBullet(Vector2 position, Vector2 direction, float speed, int damage)
    {
        var bullet = _bulletFactory.CreateBullet(position, direction, speed, damage);
        _bullets.Add(bullet);
    }

    private static bool IsColliding(Bullet bullet, Enemy enemy)
    {
        return bullet.Collider.Intersects(enemy.Collider);
    }
}