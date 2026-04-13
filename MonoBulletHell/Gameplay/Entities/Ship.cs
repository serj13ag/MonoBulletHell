using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Core;
using MonoBulletHell.Core.Graphics;
using MonoBulletHell.Gameplay.Interfaces;
using MonoBulletHell.Gameplay.Services;
using MonoBulletHell.Helpers;
using MonoBulletHell.Services;

namespace MonoBulletHell.Gameplay.Entities;

public class Ship : BaseEntity, IEntityWithCollider
{
    private const float ShipSpriteScale = 2f;
    private const float CoreSpriteScale = 1f;

    private const float ColliderRadius = 10f;

    private const int Health = 3; // TODO: to config
    private const float DamageImmuneCooldown = 2f; // TODO: to config

    private const float ShootCooldown = 0.05f; // TODO: to config
    private const float MoveSpeed = 400f; // TODO: to config

    private const float BulletSpeed = 1200f; // TODO: to config
    private const int BulletDamage = 1; // TODO: to config

    private const float FlashEffectSpeed = 6f;

    private readonly Vector2 _bulletSpawnOffset = new Vector2(0f, -32f);

    private readonly IInputService _inputService;
    private readonly IDebugService _debugService;
    private readonly ITimeService _timeService;
    private readonly IBulletService _bulletService;

    private readonly Sprite _shipSprite;
    private readonly Sprite _coreSprite;
    private readonly Effect _flashEffect;

    private Circle _collider;
    private float _timeTillCanShoot;
    private int _currentHealth = Health;
    private bool _isImmune;
    private float _timeTillDisableImmunity;
    private float _flashEffectAmount;

    public Circle Collider => _collider;
    public bool IsImmune => _isImmune;

    public event EventHandler<EventArgs> OnDestroyed;

    public Ship(IInputService inputService, IDebugService debugService, ITimeService timeService, IBulletService bulletService,
        IContentService contentService)
    {
        _inputService = inputService;
        _debugService = debugService;
        _timeService = timeService;
        _bulletService = bulletService;

        _flashEffect = contentService.GetFlashEffect();

        _shipSprite = GetShipSprite(contentService);
        _coreSprite = GetCoreSprite(contentService);
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

    public void Draw(SpriteBatch spriteBatch)
    {
        if (_isImmune)
        {
            _flashEffect.Parameters["flashAmount"].SetValue(_flashEffectAmount); // TODO: refactor? 

            spriteBatch.Begin(samplerState: Constants.SamplerState, effect: _flashEffect);
        }
        else
        {
            spriteBatch.Begin(samplerState: Constants.SamplerState);
        }

        _shipSprite.Draw(spriteBatch, Position, Rotation);
        _coreSprite.Draw(spriteBatch, Position, Rotation);

        spriteBatch.End();
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
            var newPosition = Position + inputDirection * MoveSpeed * deltaTime;
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
            _bulletService.SpawnBullet(Position + _bulletSpawnOffset, -Vector2.UnitY, BulletSpeed, BulletDamage, true);
            _timeTillCanShoot += ShootCooldown;
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
        _timeTillDisableImmunity = DamageImmuneCooldown;
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

    private static Sprite GetShipSprite(IContentService contentService)
    {
        var sprite = contentService.CreateSprite("ship");
        sprite.CenterOrigin();
        sprite.Scale = new Vector2(ShipSpriteScale, ShipSpriteScale);
        sprite.Color = Constants.Colors.ShipColor;
        return sprite;
    }

    private static Sprite GetCoreSprite(IContentService contentService)
    {
        var sprite = contentService.CreateSprite("shipCore");
        sprite.CenterOrigin();
        sprite.Color = Constants.Colors.ShipCoreColor;
        sprite.Scale = new Vector2(CoreSpriteScale, CoreSpriteScale);
        return sprite;
    }
}