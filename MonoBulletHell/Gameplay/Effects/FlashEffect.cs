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

        SetFlashAmountParameter(1f);
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
        var timeInCycle = _elapsedTime % cycleDuration;

        float flashAmount;
        if (timeInCycle < _fadeTime)
        {
            // First half: 1 → 0
            var t = timeInCycle / _fadeTime;
            flashAmount = 1f - t;
        }
        else
        {
            // Second half: 0 → 1
            var t = (timeInCycle - _fadeTime) / _fadeTime;
            flashAmount = t;
        }

        SetFlashAmountParameter(flashAmount);
    }

    public void Deactivate()
    {
        _isActive = false;
        SetFlashAmountParameter(0f);
    }

    private void SetFlashAmountParameter(float flashAmount)
    {
        _effect.Parameters["flashAmount"].SetValue(flashAmount);
    }
}