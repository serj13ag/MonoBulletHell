using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoBulletHell.AnimatedValues;
using Newtonsoft.Json;

namespace MonoBulletHell.Data;

[Serializable]
public class EmitterConfig
{
    public List<EmitterData> Emitters { get; set; }
}

[Serializable]
public class EmitterData
{
    public string Name { get; set; }

    public Vector2 Offset { get; set; }

    [JsonConverter(typeof(AnimatedFloatConverter))]
    public IAnimatedFloat RoundsPerSecond { get; set; } = new ConstantAnimatedFloat(0f);

    [JsonConverter(typeof(AnimatedFloatConverter))]
    public IAnimatedFloat StartingAngle { get; set; } = new ConstantAnimatedFloat(0f);

    // Bullet

    [JsonConverter(typeof(AnimatedFloatConverter))]
    public IAnimatedFloat BulletSpeed { get; set; } = new ConstantAnimatedFloat(0f);

    [JsonConverter(typeof(AnimatedFloatConverter))]
    public IAnimatedFloat BulletAcceleration { get; set; } = new ConstantAnimatedFloat(0f);

    [JsonConverter(typeof(AnimatedFloatConverter))]
    public IAnimatedFloat BulletAngularVelocity { get; set; } = new ConstantAnimatedFloat(0f);

    // Lines

    public int NumberOfLines { get; set; }

    [JsonConverter(typeof(AnimatedFloatConverter))]
    public IAnimatedFloat AngleBetweenLines { get; set; } = new ConstantAnimatedFloat(0f);

    public int NumberOfBulletsPerLine { get; set; }

    [JsonConverter(typeof(AnimatedFloatConverter))]
    public IAnimatedFloat AngleBetweenBulletsInLine { get; set; } = new ConstantAnimatedFloat(0f);

    // Spin

    [JsonConverter(typeof(AnimatedFloatConverter))]
    public IAnimatedFloat SpinPerSecond { get; set; } = new ConstantAnimatedFloat(0f);
}