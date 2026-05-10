using System;
using Microsoft.Xna.Framework;
using MonoBulletHell.Audio;
using MonoBulletHell.Core.Graphics;
using MonoBulletHell.Core.Physics;
using MonoBulletHell.Data.Configs;
using MonoBulletHell.Data.DTOs;
using MonoBulletHell.Gameplay.Effects;
using MonoBulletHell.Gameplay.Rendering;
using MonoBulletHell.Gameplay.Services;
using MonoBulletHell.Helpers;
using MonoBulletHell.Services;

namespace MonoBulletHell.Gameplay.Entities;

public class Ship : BaseEntity
{
    private const float ColliderRadius = 5f;
    private const float FlashEffectFadeTime = 0.25f;

    private readonly Vector2 _bulletSpawnOffset = new Vector2(0f, -20f);

    private readonly IInputActionService _inputActionService;
    private readonly IDebugService _debugService;
    private readonly ITimeService _timeService;
    private readonly IBulletService _bulletService;
    private readonly ISoundService _soundService;

    private readonly PlayerConfig _playerConfig;

    private readonly CircleCollider _collider;

    private readonly Sprite _shipSprite;
    private readonly Sprite _shipCoreSprite;
    private readonly FlashEffect _flashEffect;

    private float _timeTillCanShoot;
    private int _currentHealth;
    private bool _isImmune;
    private float _timeTillDisableImmunity;

    public CircleCollider Collider => _collider;
    public bool IsImmune => _isImmune;

    public event EventHandler<EventArgs> OnDestroyed;

    public Ship(IInputActionService inputActionService, IDebugService debugService, ITimeService timeService,
        IBulletService bulletService, IContentService contentService, ISoundService soundService, PlayerConfig playerConfig)
    {
        _inputActionService = inputActionService;
        _debugService = debugService;
        _timeService = timeService;
        _bulletService = bulletService;
        _soundService = soundService;

        _playerConfig = playerConfig;

        _collider = new CircleCollider(new Vector2(0f, 5f), ColliderRadius);

        _flashEffect = contentService.GetFlashEffect();
        _shipSprite = GetShipSprite(contentService, playerConfig);
        _shipCoreSprite = GetShipCoreSprite(contentService, playerConfig);
    }

    public void InitializeAt(Vector2 position)
    {
        Position = position;

        _currentHealth = _playerConfig.Health;
        _isImmune = false;
        _timeTillDisableImmunity = 0f;
        _timeTillCanShoot = 0f;

        _flashEffect.Deactivate();
    }

    public void Update()
    {
        var deltaTime = _timeService.DeltaTime;
        HandleMovement(deltaTime);
        HandleShooting(deltaTime);
        HandleImmunity(deltaTime);

        _flashEffect.Update(deltaTime);

        _collider.Update(Position);
        _debugService.DrawCircle(_collider.Center, _collider.Radius, Color.GreenYellow, 2f, 10);
    }

    public void Render(IRenderService renderService)
    {
        renderService.AddSprite(_shipSprite, Position, Rotation, Layer.Ship, _flashEffect.ActiveEffect);
        renderService.AddSprite(_shipCoreSprite, Position, Rotation, Layer.Ship, _flashEffect.ActiveEffect);
    }

    public void TakeDamage(int damage)
    {
        if (_isImmune)
        {
            return;
        }

        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            OnDestroyed?.Invoke(this, EventArgs.Empty);
            return;
        }

        EnableImmunity();
        _soundService.PlaySoundEffect(SfxType.PlayerDamaged);
    }

    private void HandleMovement(float deltaTime)
    {
        if (HasDirectionalInput(out var inputDirection))
        {
            var speed = _inputActionService.Focus() ? _playerConfig.FocusSpeed : _playerConfig.Speed;
            var newPosition = Position + inputDirection * speed * deltaTime;
            ScreenHelper.ClampToVirtualBounds(ref newPosition);
            Position = newPosition;
        }
    }

    private void HandleShooting(float deltaTime)
    {
        if (_timeTillCanShoot > 0f)
        {
            _timeTillCanShoot -= deltaTime;
        }

        if (_inputActionService.Shoot() && _timeTillCanShoot <= 0f)
        {
            var bulletDto = new BulletDTO()
            {
                Position = Position + _bulletSpawnOffset,
                Direction = -Vector2.UnitY,
                Speed = _playerConfig.BulletSpeed,
                IsPlayer = true,
            };
            _bulletService.SpawnBullet(in bulletDto);
            _soundService.PlaySoundEffect(SfxType.PlayerShoot);
            _timeTillCanShoot += _playerConfig.ShootCooldown;
        }
    }

    private void HandleImmunity(float deltaTime)
    {
        if (!_isImmune)
        {
            return;
        }

        _timeTillDisableImmunity -= deltaTime;
        if (_timeTillDisableImmunity <= 0f)
        {
            DisableImmunity();
        }
    }

    private void EnableImmunity()
    {
        _isImmune = true;
        _timeTillDisableImmunity = _playerConfig.DamageImmuneCooldown;

        _flashEffect.Activate(_timeTillDisableImmunity, FlashEffectFadeTime);
    }

    private void DisableImmunity()
    {
        _isImmune = false;
        _timeTillDisableImmunity = 0f;
    }

    private bool HasDirectionalInput(out Vector2 inputDirection)
    {
        inputDirection = Vector2.Zero;

        if (_inputActionService.MoveUp())
        {
            inputDirection.Y -= 1f;
        }

        if (_inputActionService.MoveDown())
        {
            inputDirection.Y += 1f;
        }

        if (_inputActionService.MoveLeft())
        {
            inputDirection.X -= 1f;
        }

        if (_inputActionService.MoveRight())
        {
            inputDirection.X += 1f;
        }

        if (inputDirection == Vector2.Zero)
        {
            return false;
        }

        inputDirection.Normalize();
        return true;
    }

    private static Sprite GetShipSprite(IContentService contentService, PlayerConfig playerConfig)
    {
        var sprite = contentService.CreateShipSprite(playerConfig.SpriteName);
        sprite.CenterOrigin();
        sprite.Color = contentService.GetColorConfig().PlayerShip;
        return sprite;
    }

    private static Sprite GetShipCoreSprite(IContentService contentService, PlayerConfig playerConfig)
    {
        var sprite = contentService.CreateShipSprite(playerConfig.CoreSpriteName);
        sprite.CenterOrigin();
        sprite.Color = contentService.GetColorConfig().PlayerShipCore;
        return sprite;
    }
}