using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MonoBulletHell.AnimatedValues;

public class CurveAnimatedFloat : IAnimatedFloat
{
    private readonly bool _looped;
    private readonly float _duration;
    private readonly List<CurveKeyframeData> _keyframes;

    public CurveAnimatedFloat(CurveAnimatedFloatData data)
    {
        _duration = data.Duration;
        _looped = data.Looped;
        _keyframes = data.Keyframes.OrderBy(k => k.Time).ToList();
    }

    public float Evaluate(float time)
    {
        if (_keyframes.Count == 0)
        {
            return 0f;
        }

        if (_looped)
        {
            time %= _duration;
        }
        else
        {
            time = Math.Clamp(time, 0, _duration);
        }

        for (var i = 0; i < _keyframes.Count - 1; i++)
        {
            var a = _keyframes[i];
            var b = _keyframes[i + 1];

            if (time >= a.Time && time <= b.Time)
            {
                var t = (time - a.Time) / (b.Time - a.Time);
                return MathHelper.Lerp(a.Value, b.Value, t);
            }
        }

        return _keyframes[^1].Value;
    }
}