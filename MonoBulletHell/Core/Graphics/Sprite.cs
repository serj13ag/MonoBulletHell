using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoBulletHell.Core.Graphics;

public class Sprite
{
    private readonly TextureRegion _region;

    public Color Color { get; set; } = Color.White;
    public float Rotation { get; set; } = 0.0f;
    public Vector2 Scale { get; set; } = Vector2.One;
    public Vector2 Origin { get; set; } = Vector2.Zero;
    public SpriteEffects Effects { get; set; } = SpriteEffects.None;
    public float LayerDepth { get; set; } = 0.0f;

    public float Width => _region.Width * Scale.X;
    public float Height => _region.Height * Scale.Y;

    public Sprite(TextureRegion region)
    {
        _region = region;
    }

    public void CenterOrigin()
    {
        Origin = new Vector2(_region.Width, _region.Height) * 0.5f;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        _region.Draw(spriteBatch, position, Color, Rotation, Origin, Scale, Effects, LayerDepth);
    }
}