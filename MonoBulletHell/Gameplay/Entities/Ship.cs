using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Core;
using MonoBulletHell.Core.Graphics;
using MonoBulletHell.Data;
using MonoBulletHell.Gameplay.Interfaces;
using MonoBulletHell.Gameplay.Services;
using MonoBulletHell.Helpers;
using MonoBulletHell.Services;

namespace MonoBulletHell.Gameplay.Entities;

public class Ship : BaseEntity, IEntityWithCollider
{
    private const float FlashEffectSpeed = 6f;
    private const float ColliderRadius = 10f;

    private readonly Vector2 _bulletSpawnOffset = new Vector2(0f, -32f);

    private readonly IInputService _inputService;
    private readonly IDebugService _debugService;
    private readonly ITimeService _timeService;
    private readonly IBulletService _bulletService;

    private readonly PlayerConfig _playerConfig;

    private readonly Sprite _shipSprite;
    private readonly Effect _flashEffect;

    private Circle _collider;
    private float _timeTillCanShoot;
    private int _currentHealth;
    private bool _isImmune;
    private float _timeTillDisableImmunity;
    private float _flashEffectAmount;

    public Circle Collider => _collider;
    public bool IsImmune => _isImmune;

    public event EventHandler<EventArgs> OnDestroyed;

    public Ship(IInputService inputService, IDebugService debugService, ITimeService timeService, IBulletService bulletService,
        IContentService contentService, PlayerConfig playerConfig)
    {
        _inputService = inputService;
        _debugService = debugService;
        _timeService = timeService;
        _bulletService = bulletService;

        _playerConfig = playerConfig;

        _flashEffect = contentService.GetFlashEffect();

        _shipSprite = GetShipSprite(contentService, playerConfig);
    }

    public void InitializeAt(Vector2 position)
    {
        Position = position;

        _currentHealth = _playerConfig.Health;
        _isImmune = false;
        _timeTillDisableImmunity = 0f;
        _timeTillCanShoot = 0f;
        _flashEffectAmount = 0f;
    }

    public void Update()
    {
        var deltaTime = _timeService.DeltaTime;
        HandleMovement(deltaTime);
        HandleShooting(deltaTime);
        HandleImmunity(deltaTime);

        _collider = new Circle(Position.X, Position.Y, ColliderRadius);
        _debugService.DrawCircle(_collider.Location, _collider.Radius, Color.GreenYellow, 2f, 10);
    }

    public void Render(IRenderService renderService)
    {
        Effect effect = null;
        if (_isImmune)
        {
            _flashEffect.Parameters["flashAmount"].SetValue(_flashEffectAmount); // TODO: refactor? 
            effect = _flashEffect;
        }

        renderService.AddSprite(_shipSprite, Position, Rotation, effect);
    }

    public void TakeDamage(int damage)
    {
        if (_isImmune)
        {
            return;
        }

        _currentHealth -= damage;

        if (_currentHealth == 0)
        {
            OnDestroyed?.Invoke(this, EventArgs.Empty);
            return;
        }

        EnableImmunity();
    }

    private void HandleMovement(float deltaTime)
    {
        if (HasDirectionalInput(out var inputDirection))
        {
            var newPosition = Position + inputDirection * _playerConfig.Speed * deltaTime;
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

        if (_inputService.Shoot() && _timeTillCanShoot <= 0f)
        {
            _bulletService.SpawnBullet(Position + _bulletSpawnOffset, -Vector2.UnitY, _playerConfig.BulletSpeed,
                Constants.BulletDamage, true);
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
        else
        {
            _flashEffectAmount = MathF.Abs(MathF.Sin(_timeTillDisableImmunity * FlashEffectSpeed));
        }
    }

    private void EnableImmunity()
    {
        _isImmune = true;
        _timeTillDisableImmunity = _playerConfig.DamageImmuneCooldown;
    }

    private void DisableImmunity()
    {
        _isImmune = false;
        _timeTillDisableImmunity = 0f;
    }

    private bool HasDirectionalInput(out Vector2 inputDirection)
    {
        inputDirection = Vector2.Zero;

        if (_inputService.MoveUp())
        {
            inputDirection.Y -= 1f;
        }

        if (_inputService.MoveDown())
        {
            inputDirection.Y += 1f;
        }

        if (_inputService.MoveLeft())
        {
            inputDirection.X -= 1f;
        }

        if (_inputService.MoveRight())
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
        sprite.Color = Constants.Colors.BackgroundHighlight;
        return sprite;
    }
}