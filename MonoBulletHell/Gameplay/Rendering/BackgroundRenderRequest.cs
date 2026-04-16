using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoBulletHell.Gameplay.Rendering;

public class BackgroundRenderRequest
{
    private readonly Texture2D _texture;
    private readonly Rectangle _destinationRectangle;
    private readonly Rectangle _sourceRectangle;
    private readonly Color _color;

    public SamplerState SamplerState { get; }

    public BackgroundRenderRequest(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color,
        SamplerState samplerState)
    {
        _texture = texture;
        _destinationRectangle = destinationRectangle;
        _sourceRectangle = sourceRectangle;
        _color = color;
        SamplerState = samplerState;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, _destinationRectangle, _sourceRectangle, _color);
    }
}