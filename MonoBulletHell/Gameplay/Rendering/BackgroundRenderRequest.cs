using Gum.Forms.DefaultVisuals.V3;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoBulletHell.Gameplay.Rendering;

public class BackgroundRenderRequest
{
    private static readonly Rectangle DestinationRectangle =
        new Rectangle(Point.Zero, new Point(Constants.VirtualWidth, Constants.VirtualHeight));

    private readonly Texture2D _texture;
    private readonly int _verticalOffset;

    public BackgroundRenderRequest(Texture2D texture, int verticalOffset)
    {
        _texture = texture;
        _verticalOffset = verticalOffset;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        var sourceRectangle = new Rectangle(new Point(0, _verticalOffset), DestinationRectangle.Size);
        spriteBatch.Draw(_texture, DestinationRectangle, sourceRectangle, Constants.Colors.BackgroundColor.Adjust(-20f));
    }
}