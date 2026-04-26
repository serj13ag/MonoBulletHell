using System;
using Microsoft.Xna.Framework.Graphics;

namespace MonoBulletHell.Gameplay.Effects;

public class FlashEffect
{
    private readonly Effect _effect;

    private float _activeTime;
    private bool _isActive;
    private float _fadeTime;
    private float _elapsedTime;

    public Effect ActiveEffect => _isActive ? _effect : null;

    public FlashEffect(Effect effect)
    {
        _effect = effect;
    }

    public void Activate(float activeTime, float fadeTime)
    {
        _activeTime = activeTime;
        _fadeTime = fadeTime;
        _elapsedTime = 0f;
        _isActive = true;

        _effect.Parameters["flashAmount"].SetValue(1f);
    }

    public void Update(float deltaTime)
    {
        if (!_isActive)
        {
            return;
        }

        _elapsedTime += deltaTime;

        if (_elapsedTime >= _activeTime)
        {
            Deactivate();
            return;
        }

        var cycleDuration = _fadeTime * 2f;
        var cycleProgress = _elapsedTime % cycleDuration / cycleDuration;
        // Convert progress into a triangle wave: 0 → 1 → 0 shape, then flipped to 1 → 0 → 1
        var flashAmount = MathF.Abs(2f * cycleProgress - 1f);
        _effect.Parameters["flashAmount"].SetValue(flashAmount);
    }

    public void Deactivate()
    {
        _isActive = false;
        _effect.Parameters["flashAmount"].SetValue(0f);
    }
}