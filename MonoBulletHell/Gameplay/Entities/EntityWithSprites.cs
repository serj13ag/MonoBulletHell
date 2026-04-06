using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Core.Graphics;

namespace MonoBulletHell.Gameplay.Entities;

public abstract class EntityWithSprites : BaseEntity
{
    private class SpriteEntry
    {
        public Sprite Sprite { get; }
        public Vector2 Offset { get; }

        public SpriteEntry(Sprite sprite, Vector2 offset)
        {
            Sprite = sprite;
            Offset = offset;
        }
    }

    private readonly List<SpriteEntry> _sprites = [];

    public void AddSprite(Sprite sprite, Vector2 offset)
    {
        _sprites.Add(new SpriteEntry(sprite, offset));
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var sprite in _sprites)
        {
            sprite.Sprite.Draw(spriteBatch, Position + sprite.Offset, Rotation);
        }
    }
}