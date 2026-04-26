using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Core.Graphics;

namespace MonoBulletHell.Gameplay.Rendering;

public class SpriteRenderRequest
{
    private readonly Sprite _sprite;
    private readonly Vector2 _position;
    private readonly float _rotation;

    public Layer Layer { get; }
    public Effect Effect { get; }
    public float PositionY => _position.Y;

    public SpriteRenderRequest(Sprite sprite, Vector2 position, float rotation, Layer layer, Effect effect = null)
    {
        _sprite = sprite;
        _position = position;
        _rotation = rotation;
        Layer = layer;
        Effect = effect;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _sprite.Draw(spriteBatch, Vector2.Floor(_position), _rotation);
    }
}