using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Core.Graphics;

namespace MonoBulletHell.Gameplay.Entities;

public class Particle : BaseEntity
{
    private readonly AnimatedSprite _animatedSprite;

    public bool Finished => _animatedSprite.NumberOfRepeats > 0; // TODO: rework?

    public Particle(AnimatedSprite animatedSprite)
    {
        _animatedSprite = animatedSprite;
    }

    public void Update(GameTime deltaTime)
    {
        _animatedSprite.Update(deltaTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin(samplerState: Constants.SamplerState); // TODO: fix
        _animatedSprite.Draw(spriteBatch, Position, Rotation);
        spriteBatch.End();
    }
}