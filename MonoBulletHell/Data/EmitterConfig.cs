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
    public float RoundsPerSecond { get; set; }

    [JsonConverter(typeof(AnimatedFloatConverter))]
    public IAnimatedFloat StartingAngle { get; set; }

    public float BulletSpeed { get; set; }
    public float BulletAcceleration { get; set; }
    public float BulletAngularVelocity { get; set; }

    public int NumberOfLines { get; set; }
    public float AngleBetweenLines { get; set; }
    public int NumberOfBulletsPerLine { get; set; }
    public float AngleBetweenBulletsInLine { get; set; }

    public float SpinPerSecond { get; set; }
}