using System;
using Microsoft.Xna.Framework;
using MonoBulletHell.Audio;
using MonoBulletHell.Core.Graphics;
using MonoBulletHell.Core.Physics;
using MonoBulletHell.Data.Configs;
using MonoBulletHell.Data.DTOs;
using MonoBulletHell.Gameplay.Effects;
using MonoBulletHell.Gameplay.Entities.Emitters;
using MonoBulletHell.Gameplay.Movement;
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

    private IMovement _movement;
    private IBulletEmitter _bulletEmitter;

    private int _currentHealth;

    public CircleCollider Collider => _collider;

    public bool IsDead => _currentHealth <= 0;
    public bool PathIsFinished => _movement.IsFinished;

    public event Action<HealthChangedDTO> HealthChanged;

    public Enemy(IDebugService debugService, ITimeService timeService, IContentService contentService, ISoundService soundService,
        EnemyData enemyData, IMovement movement, IBulletEmitter bulletEmitter)
    {
        _debugService = debugService;
        _timeService = timeService;
        _soundService = soundService;

        _health = enemyData.Health;
        _currentHealth = enemyData.Health;

        _movement = movement;
        Position = _movement.Position;
        _movement.ShootingDisabledChanged += OnMovementShootingDisabledChanged;

        _collider = new CircleCollider(enemyData.ColliderOffset, enemyData.ColliderRadius);

        _bulletEmitter = bulletEmitter;
        _bulletEmitter.Position = Position;
        _bulletEmitter.SetShootingDisabled(_movement.ShootingDisabled);

        _sprite = GetEnemySprite(contentService, enemyData.SpriteName);
        _spriteOffset = enemyData.SpriteOffset;
        _flashEffect = contentService.GetFlashEffect();
    }

    public void Update()
    {
        _movement.Update(_timeService.DeltaTime);
        Position = _movement.Position;

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

    public void ChangeMovement(IMovement movement)
    {
        if (_movement != null)
        {
            _movement.ShootingDisabledChanged -= OnMovementShootingDisabledChanged;
        }

        _movement = movement;
        Position = _movement.Position;
        _bulletEmitter.SetShootingDisabled(_movement.ShootingDisabled);

        _movement.ShootingDisabledChanged += OnMovementShootingDisabledChanged;
    }

    public void ChangeEmitter(IBulletEmitter emitter)
    {
        _bulletEmitter = emitter;
        _bulletEmitter.Position = Position;
        _bulletEmitter.SetShootingDisabled(_movement.ShootingDisabled);
    }

    private void OnMovementShootingDisabledChanged(bool shootingDisabled)
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