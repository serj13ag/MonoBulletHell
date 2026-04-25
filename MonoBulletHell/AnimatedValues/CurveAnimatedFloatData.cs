using System;
using System.Collections.Generic;

namespace MonoBulletHell.AnimatedValues;

[Serializable]
public class CurveAnimatedFloatData
{
    public float Duration { get; set; }
    public bool Looped { get; set; }
    public List<CurveKeyframeData> Keyframes { get; set; }
}

[Serializable]
public class CurveKeyframeData
{
    public float Time { get; set; }
    public float Value { get; set; }
}