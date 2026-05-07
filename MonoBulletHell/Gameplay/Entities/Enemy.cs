using System;
using Microsoft.Xna.Framework;
using MonoBulletHell.Core.Graphics;
using MonoBulletHell.Core.Physics;
using MonoBulletHell.Data.Configs;
using MonoBulletHell.Data.DTOs;
using MonoBulletHell.Enums;
using MonoBulletHell.Gameplay.Effects;
using MonoBulletHell.Gameplay.Entities.Emitters;
using MonoBulletHell.Gameplay.Entities.PathBlocks;
using MonoBulletHell.Gameplay.Rendering;
using MonoBulletHell.Gameplay.Services;
using MonoBulletHell.Services;

namespace MonoBulletHell.Gameplay.Entities;

public class Enemy : BaseEntity
{
    private const float FlashEffectDuration = 0.2f;

    private readonly IDebugService _debugService;
    private readonly ITimeService _timeService;
    private readonly ISoundService _soundService;

    private readonly int _health;
    private readonly CircleCollider _collider;

    private readonly Sprite _sprite;
    private readonly Vector2 _spriteOffset;
    private readonly FlashEffect _flashEffect;

    private IPathBlock _pathBlock;
    private IBulletEmitter _bulletEmitter;

    private int _currentHealth;

    public CircleCollider Collider => _collider;

    public bool IsDead => _currentHealth <= 0;
    public bool PathIsFinished => _pathBlock.IsFinished;

    public event Action<HealthChangedDTO> HealthChanged;

    public Enemy(IDebugService debugService, ITimeService timeService, IContentService contentService, ISoundService soundService,
        EnemyData enemyData, IPathBlock pathBlock, IBulletEmitter bulletEmitter)
    {
        _debugService = debugService;
        _timeService = timeService;
        _soundService = soundService;

        _health = enemyData.Health;
        _currentHealth = enemyData.Health;

        _pathBlock = pathBlock;
        Position = _pathBlock.Position;
        _pathBlock.ShootingDisabledChanged += OnPathShootingDisabledChanged;

        _collider = new CircleCollider(enemyData.ColliderOffset, enemyData.ColliderRadius);

        _bulletEmitter = bulletEmitter;
        _bulletEmitter.Position = Position;
        _bulletEmitter.SetShootingDisabled(_pathBlock.ShootingDisabled);

        _sprite = GetEnemySprite(contentService, enemyData.SpriteName);
        _spriteOffset = enemyData.SpriteOffset;
        _flashEffect = contentService.GetFlashEffect();
    }

    public void Update()
    {
        _pathBlock.Update(_timeService.DeltaTime);
        Position = _pathBlock.Position;

        _collider.Update(Position);
        _debugService.DrawCircle(_collider.Center, _collider.Radius, Color.GreenYellow, 2f, 10);

        _bulletEmitter.Position = Position;
        _bulletEmitter.Update(_timeService.DeltaTime);

        _flashEffect.Update(_timeService.DeltaTime);
    }

    public void Render(IRenderService renderService)
    {
        renderService.AddSprite(_sprite, Position + _spriteOffset, Rotation, Layer.Enemies, _flashEffect.ActiveEffect);
    }

    public void TakeDamage(int damage)
    {
        var prevHealth = _currentHealth;

        _currentHealth -= damage;
        _currentHealth = Math.Max(0, _currentHealth);

        if (_currentHealth > 0)
        {
            _flashEffect.Activate(FlashEffectDuration, FlashEffectDuration);
            _soundService.PlaySoundEffect(SfxType.EnemyDamaged);
        }

        HealthChanged?.Invoke(new HealthChangedDTO()
        {
            MaxHealth = _health,
            PreviousHealth = prevHealth,
            NewHealth = _currentHealth,
        });
    }

    public void ChangePathBlock(IPathBlock pathBlock)
    {
        if (_pathBlock != null)
        {
            _pathBlock.ShootingDisabledChanged -= OnPathShootingDisabledChanged;
        }

        _pathBlock = pathBlock;
        Position = _pathBlock.Position;
        _bulletEmitter.SetShootingDisabled(_pathBlock.ShootingDisabled);

        _pathBlock.ShootingDisabledChanged += OnPathShootingDisabledChanged;
    }

    public void ChangeEmitter(IBulletEmitter emitter)
    {
        _bulletEmitter = emitter;
        _bulletEmitter.Position = Position;
        _bulletEmitter.SetShootingDisabled(_pathBlock.ShootingDisabled);
    }

    private void OnPathShootingDisabledChanged(bool shootingDisabled)
    {
        _bulletEmitter?.SetShootingDisabled(shootingDisabled);
    }

    private static Sprite GetEnemySprite(IContentService contentService, string spriteName)
    {
        var sprite = contentService.CreateShipSprite(spriteName);
        sprite.CenterOrigin();
        sprite.Color = contentService.GetColorConfig().Enemy;
        return sprite;
    }
}