using Microsoft.Xna.Framework;
using MonoBulletHell.Core.Graphics;
using MonoBulletHell.Core.Physics;
using MonoBulletHell.Data;
using MonoBulletHell.Enums;
using MonoBulletHell.Gameplay.Effects;
using MonoBulletHell.Gameplay.Entities.Emitters;
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

    private readonly IBulletEmitter _bulletEmitter;

    private readonly CircleCollider _collider;
    private readonly PathBlock _pathBlock;

    private readonly Sprite _sprite;
    private readonly FlashEffect _flashEffect;

    private int _currentHealth;

    public CircleCollider Collider => _collider;

    public bool IsDead => _currentHealth <= 0;
    public bool PathIsFinished => _pathBlock.IsFinished;

    public Enemy(IDebugService debugService, ITimeService timeService, IContentService contentService, ISoundService soundService,
        Vector2 position, PathData path, EnemyData enemyData, IBulletEmitter bulletEmitter)
    {
        _debugService = debugService;
        _timeService = timeService;
        _soundService = soundService;

        _currentHealth = enemyData.Health;

        _pathBlock = new PathBlock(path, position, enemyData.Speed);
        Position = _pathBlock.Position;

        _collider = new CircleCollider(enemyData.ColliderOffset, enemyData.ColliderRadius);

        _bulletEmitter = bulletEmitter;
        _bulletEmitter.Position = Position;
        _bulletEmitter.SetShootingDisabled(_pathBlock.ShootingDisabled);

        _sprite = GetEnemySprite(contentService, enemyData.SpriteName);
        _flashEffect = contentService.GetFlashEffect();

        _pathBlock.ShootingDisabledChanged += OnPathShootingDisabledChanged;
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
        var spriteOffset = new Vector2(0f, -10f); // TODO: refactor
        renderService.AddSprite(_sprite, Position + spriteOffset, Rotation, Layer.Enemies, _flashEffect.ActiveEffect);
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth > 0)
        {
            _flashEffect.Activate(FlashEffectDuration, FlashEffectDuration);
            _soundService.PlaySoundEffect(SfxType.EnemyDamaged);
        }
    }

    private void OnPathShootingDisabledChanged(bool shootingDisabled)
    {
        _bulletEmitter?.SetShootingDisabled(shootingDisabled);
    }

    private static Sprite GetEnemySprite(IContentService contentService, string spriteName)
    {
        var sprite = contentService.CreateShipSprite(spriteName);
        sprite.CenterOrigin();
        sprite.Color = Constants.Colors.BackgroundHighlight;
        return sprite;
    }
}