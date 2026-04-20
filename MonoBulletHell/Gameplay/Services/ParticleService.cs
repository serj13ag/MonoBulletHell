using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoBulletHell.Gameplay.Entities;

namespace MonoBulletHell.Gameplay.Services;

public interface IParticleService
{
    void Update();
    void Render(IRenderService renderService);

    void CreateBulletImpact(Vector2 position);
    void Clear();
}

public class ParticleService : IParticleService
{
    private readonly ITimeService _timeService;
    private readonly IContentService _contentService;

    private readonly List<Particle> _particles = new List<Particle>();
    private readonly List<Particle> _particlesToDestroy = new List<Particle>();

    public ParticleService(ITimeService timeService, IContentService contentService)
    {
        _timeService = timeService;
        _contentService = contentService;
    }

    public void Update()
    {
        foreach (var particle in _particles)
        {
            particle.Update(_timeService.DeltaGameTime);

            if (particle.Finished)
            {
                _particlesToDestroy.Add(particle);
            }
        }

        foreach (var particleToDestroy in _particlesToDestroy)
        {
            _particles.Remove(particleToDestroy);
        }

        _particlesToDestroy.Clear();
    }

    public void Render(IRenderService renderService)
    {
        foreach (var particle in _particles)
        {
            particle.Render(renderService);
        }
    }

    public void CreateBulletImpact(Vector2 position)
    {
        var animatedSprite = _contentService.CreateBulletAnimatedSprite("bulletImpact");
        animatedSprite.CenterOrigin();
        animatedSprite.Color = Constants.Colors.ShipProjectile;

        var particle = new Particle(animatedSprite);
        particle.Position = position;

        _particles.Add(particle);
    }

    public void Clear()
    {
        _particles.Clear();
    }
}