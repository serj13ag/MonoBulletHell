using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Core.Graphics;
using MonoBulletHell.Gameplay.Services;
using MonoBulletHell.Helpers;
using MonoBulletHell.Services;

namespace MonoBulletHell.Gameplay.GameObjects;

public class Ship
{
    private const float MoveSpeed = 400f; // TODO: to config
    private const float BulletSpeed = 1200f; // TODO: to config

    private readonly Vector2 _bulletSpawnOffset = new Vector2(0f, -32f);

    private readonly IInputService _inputService;
    private readonly ITimeService _timeService;
    private readonly IBulletService _bulletService;

    private readonly Sprite _shipSprite;
    private readonly Sprite _coreSprite;

    private Vector2 _position;

    public Vector2 Position
    {
        get => _position;
        set => _position = value;
    }

    public Ship(IInputService inputService, ITimeService timeService, IContentService contentService,
        IBulletService bulletService)
    {
        _inputService = inputService;
        _timeService = timeService;
        _bulletService = bulletService;

        _shipSprite = GetShipSprite(contentService); // TODO: init with sprites
        _coreSprite = GetCoreSprite(contentService);

        _position = new Vector2(32f, 32f);
    }

    public void Update()
    {
        if (HasDirectionalInput(out var inputDirection))
        {
            var newPosition = _position + inputDirection * MoveSpeed * _timeService.DeltaTime;
            ScreenHelper.ClampToVirtualBounds(ref newPosition);
            _position = newPosition;
        }

        if (_inputService.Shoot()) // TODO: add cooldown
        {
            _bulletService.SpawnBullet(_position + _bulletSpawnOffset, -Vector2.UnitY, BulletSpeed);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _shipSprite.Draw(spriteBatch, _position);
        _coreSprite.Draw(spriteBatch, _position);
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
        var sprite = contentService.GetShipSprite();
        sprite.CenterOrigin();
        sprite.Scale = new Vector2(4f, 4f);
        return sprite;
    }

    private static Sprite GetCoreSprite(IContentService contentService)
    {
        var sprite = contentService.GetShipCoreSprite();
        sprite.CenterOrigin();
        sprite.Color = Color.Red;
        sprite.Scale = new Vector2(2f, 2f);
        return sprite;
    }
}