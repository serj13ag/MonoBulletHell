using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Core.Graphics;
using MonoBulletHell.Services;

namespace MonoBulletHell.GameObjects;

public class Ship
{
    private const float MoveSpeed = 200f;

    private readonly InputService _inputService;
    private readonly TimeService _timeService;

    private readonly Sprite _shipSprite;
    private readonly Sprite _coreSprite;

    private Vector2 _position;

    public Ship(InputService inputService, TimeService timeService, ContentService contentService)
    {
        _inputService = inputService;
        _timeService = timeService;

        _shipSprite = GetShipSprite(contentService);
        _coreSprite = GetCoreSprite(contentService);

        _position = new Vector2(32f, 32f);
    }

    public void Update()
    {
        if (HasInput(out var inputDirection))
        {
            _position += inputDirection * MoveSpeed * _timeService.DeltaTime;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _shipSprite.Draw(spriteBatch, _position);
        _coreSprite.Draw(spriteBatch, _position);
    }

    private bool HasInput(out Vector2 inputDirection)
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

    private static Sprite GetShipSprite(ContentService contentService)
    {
        var sprite = contentService.GetShipSprite();
        sprite.CenterOrigin();
        sprite.Scale = new Vector2(4f, 4f);
        return sprite;
    }

    private static Sprite GetCoreSprite(ContentService contentService)
    {
        var sprite = contentService.GetShipCoreSprite();
        sprite.CenterOrigin();
        sprite.Color = Color.Red;
        sprite.Scale = new Vector2(2f, 2f);
        return sprite;
    }
}