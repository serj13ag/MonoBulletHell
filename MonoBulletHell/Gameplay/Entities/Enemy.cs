using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Core;
using MonoBulletHell.Core.Graphics;
using MonoBulletHell.Gameplay.Interfaces;
using MonoBulletHell.Gameplay.Services;
using MonoBulletHell.Services;

namespace MonoBulletHell.Gameplay.Entities;

public class Enemy : IEntityWithCollider
{
    private const float ShootCooldown = 0.5f;
    private const float BulletSpeed = 800;
    private const int BulletDamage = 1;

    private const float ColliderRadius = 45f;
    private const float SpriteScale = 8f;
    private static readonly Vector2 SpriteOffset = new Vector2(0f, -25f);

    private readonly IDebugService _debugService;
    private readonly ITimeService _timeService;
    private readonly IBulletService _bulletService;

    private readonly Sprite _sprite;

    private Vector2 _position;
    private Circle _collider;

    private float _timeTillShoot;

    public Vector2 Position
    {
        get => _position;
        set => _position = value;
    }

    public Circle Collider => _collider;

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
        HandleShooting();

        _collider = new Circle(_position.X, _position.Y, ColliderRadius);
        _debugService.DrawCircle(_collider.Location, _collider.Radius, Color.GreenYellow, 2f, 10);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _sprite.Draw(spriteBatch, _position + SpriteOffset);
    }

    public void TakeDamage(int damage)
    {
        // TODO: implement
    }

    private void HandleShooting()
    {
        if (_timeTillShoot <= 0f)
        {
            _bulletService.SpawnBullet(_position, Vector2.UnitY, BulletSpeed, BulletDamage, false);
            _timeTillShoot += ShootCooldown;
        }
        else
        {
            _timeTillShoot -= _timeService.DeltaTime;
        }
    }

    private static Sprite GetEnemySprite(IContentService contentService)
    {
        var sprite = contentService.CreateSprite("enemy");
        sprite.CenterOrigin();
        sprite.Scale = new Vector2(SpriteScale, SpriteScale);
        return sprite;
    }
}