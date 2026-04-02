using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Core.Graphics;

namespace MonoBulletHell.GameObjects;

public class Ship
{
    private readonly Sprite _sprite;

    private Vector2 _position;

    public Ship(Sprite sprite)
    {
        _sprite = sprite;
        _position = new Vector2(32f, 32f);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _sprite.Draw(spriteBatch, _position);
    }
}