using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Core;
using MonoBulletHell.Core.Graphics;
using MonoBulletHell.Data;
using MonoBulletHell.Gameplay.Interfaces;
using MonoBulletHell.Gameplay.Services;
using MonoBulletHell.Services;

namespace MonoBulletHell.Gameplay.Entities;

public class Enemy : BaseEntity, IEntityWithCollider
{
    private const float FlashEffectDuration = 0.2f;
    private const float ColliderRadius = 20f;

    private readonly IDebugService _debugService;
    private readonly ITimeService _timeService;
    private readonly IBulletService _bulletService;

    private readonly EnemyData _enemyData;

    private readonly Sprite _sprite;
    private readonly Effect _flashEffect;

    private readonly PathBlock _pathBlock;

    private Circle _collider;
    private int _currentHealth;
    private float _timeTillShoot;

    private float _timeTillEndFlashEffect;
    private float _flashEffectAmount;

    public Circle Collider => _collider;

    public bool IsDead => _currentHealth <= 0;
    public bool PathIsFinished => _pathBlock.IsFinished;

    public Enemy(IDebugService debugService, ITimeService timeService, IBulletService bulletService,
        IContentService contentService, Vector2 position, PathData path, EnemyData enemyData)
    {
        _debugService = debugService;
        _timeService = timeService;
        _bulletService = bulletService;

        _enemyData = enemyData;

        _currentHealth = enemyData.Health;

        _sprite = GetEnemySprite(contentService, enemyData.SpriteName);
        _flashEffect = contentService.GetFlashEffect();

        _pathBlock = new PathBlock(path, position, enemyData.Speed);
        Position = _pathBlock.Position;
    }

    public void Update()
    {
        _pathBlock.Update(_timeService.DeltaTime);
        Position = _pathBlock.Position;

        if (_enemyData.ShootCooldown > 0)
        {
            HandleShooting();
        }

        HandleFlashEffect();

        _collider = new Circle(Position.X, Position.Y, ColliderRadius);
        _debugService.DrawCircle(_collider.Location, _collider.Radius, Color.GreenYellow, 2f, 10);
    }

    public void Render(IRenderService renderService)
    {
        Effect effect = null;
        if (_timeTillEndFlashEffect > 0)
        {
            _flashEffect.Parameters["flashAmount"].SetValue(_flashEffectAmount); // TODO: refactor? 
            effect = _flashEffect;
        }

        var spriteOffset = new Vector2(0f, -10f); // TODO: refactor
        renderService.AddSprite(_sprite, Position + spriteOffset, Rotation, effect);
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth > 0)
        {
            _timeTillEndFlashEffect = FlashEffectDuration;
        }
    }

    private void HandleShooting()
    {
        if (_timeTillShoot <= 0f)
        {
            if (_pathBlock.ShootingDisabled)
            {
                return;
            }

            _bulletService.SpawnBullet(Position, Vector2.UnitY, _enemyData.BulletSpeed, Constants.BulletDamage, false);
            _timeTillShoot += _enemyData.ShootCooldown;
        }
        else
        {
            _timeTillShoot -= _timeService.DeltaTime;
        }
    }

    private void HandleFlashEffect()
    {
        if (_timeTillEndFlashEffect > 0)
        {
            _timeTillEndFlashEffect -= _timeService.DeltaTime;
            _flashEffectAmount = _timeTillEndFlashEffect / FlashEffectDuration;
        }
    }

    private static Sprite GetEnemySprite(IContentService contentService, string spriteName)
    {
        var sprite = contentService.CreateShipSprite(spriteName);
        sprite.CenterOrigin();
        sprite.Color = Constants.Colors.BackgroundHighlight;
        return sprite;
    }
}