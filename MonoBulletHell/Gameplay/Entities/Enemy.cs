using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Core;
using MonoBulletHell.Core.Graphics;
using MonoBulletHell.Gameplay.Interfaces;
using MonoBulletHell.Gameplay.Services;
using MonoBulletHell.Services;

namespace MonoBulletHell.Gameplay.Entities;

public class Enemy : BaseEntity, IEntityWithCollider
{
    private const float FlashEffectDuration = 0.2f;

    private const int Health = 10; // TODO: to config
    private const float ShootCooldown = 0.5f;
    private const float BulletSpeed = 300f;
    private const int BulletDamage = 1;

    private const float ColliderRadius = 45f;

    private readonly IDebugService _debugService;
    private readonly ITimeService _timeService;
    private readonly IBulletService _bulletService;

    private readonly Sprite _sprite;
    private readonly Effect _flashEffect;

    private Circle _collider;
    private int _currentHealth = Health;
    private float _timeTillShoot;

    private float _timeTillEndFlashEffect;
    private float _flashEffectAmount;

    public Circle Collider => _collider;

    public event EventHandler<EventArgs> OnDestroyed;

    public Enemy(IDebugService debugService, ITimeService timeService, IBulletService bulletService,
        IContentService contentService)
    {
        _debugService = debugService;
        _timeService = timeService;
        _bulletService = bulletService;

        _sprite = GetEnemySprite(contentService);
        _flashEffect = contentService.GetFlashEffect();
    }

    public void Update()
    {
        HandleShooting();
        HandleFlashEffect();

        _collider = new Circle(Position.X, Position.Y, ColliderRadius);
        _debugService.DrawCircle(_collider.Location, _collider.Radius, Color.GreenYellow, 2f, 10);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (_timeTillEndFlashEffect > 0)
        {
            _flashEffect.Parameters["flashAmount"].SetValue(_flashEffectAmount); // TODO: refactor? 

            spriteBatch.Begin(samplerState: Constants.SamplerState, effect: _flashEffect);
        }
        else
        {
            spriteBatch.Begin(samplerState: Constants.SamplerState);
        }

        var spriteOffset = new Vector2(0f, -25f); // TODO: refactor
        _sprite.Draw(spriteBatch, Position + spriteOffset, Rotation);

        spriteBatch.End();
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            OnDestroyed?.Invoke(this, EventArgs.Empty);
            return;
        }

        _timeTillEndFlashEffect = FlashEffectDuration;
    }

    private void HandleShooting()
    {
        if (_timeTillShoot <= 0f)
        {
            _bulletService.SpawnBullet(Position, Vector2.UnitY, BulletSpeed, BulletDamage, false);
            _timeTillShoot += ShootCooldown;
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

    private static Sprite GetEnemySprite(IContentService contentService)
    {
        var sprite = contentService.CreateSprite("enemy");
        sprite.CenterOrigin();
        sprite.Scale = new Vector2(4f, 4f);
        sprite.Color = Constants.Colors.EnemyColor;
        return sprite;
    }
}