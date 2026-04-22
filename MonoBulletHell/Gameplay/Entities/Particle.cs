using Microsoft.Xna.Framework;
using MonoBulletHell.Core.Graphics;
using MonoBulletHell.Gameplay.Rendering;
using MonoBulletHell.Gameplay.Services;

namespace MonoBulletHell.Gameplay.Entities;

public class Particle : BaseEntity
{
    private readonly AnimatedSprite _animatedSprite;

    public bool Finished => _animatedSprite.CompletedLoops > 0;

    public Particle(AnimatedSprite animatedSprite)
    {
        _animatedSprite = animatedSprite;
    }

    public void Update(GameTime deltaTime)
    {
        _animatedSprite.Update(deltaTime);
    }

    public void Render(IRenderService renderService)
    {
        renderService.AddSprite(_animatedSprite, Position, Rotation, Layer.Particles);
    }
}