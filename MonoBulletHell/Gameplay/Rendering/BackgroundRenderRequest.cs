using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.App;

namespace MonoBulletHell.Gameplay.Rendering;

public class BackgroundRenderRequest
{
    private static readonly Rectangle DestinationRectangle =
        new Rectangle(Point.Zero, new Point(GameConstants.VirtualWidth, GameConstants.VirtualHeight));

    private readonly Texture2D _texture;
    private readonly int _verticalOffset;
    private readonly Color _color;

    public BackgroundRenderRequest(Texture2D texture, int verticalOffset, Color color)
    {
        _texture = texture;
        _verticalOffset = verticalOffset;
        _color = color;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        var sourceRectangle = new Rectangle(new Point(0, _verticalOffset), DestinationRectangle.Size);
        spriteBatch.Draw(_texture, DestinationRectangle, sourceRectangle, _color);
    }
}