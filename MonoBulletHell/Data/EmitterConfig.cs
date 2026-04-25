using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

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
    public float BulletSpeed { get; set; }
    public float StartingAngle { get; set; }

    public int NumberOfLines { get; set; }
    public float AngleBetweenLines { get; set; }
    public int NumberOfBulletsPerLine { get; set; }
    public float AngleBetweenBulletsInLine { get; set; }
}