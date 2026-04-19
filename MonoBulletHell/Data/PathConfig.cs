using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoBulletHell.Data;

[Serializable]
public class PathConfig
{
    public List<PathData> Paths { get; set; }
}

[Serializable]
public class PathData
{
    public string Name { get; set; }
    public int Loops { get; set; }
    public List<PathPointData> Points { get; set; }
    // TODO: add inverse bool
}

[Serializable]
public class PathPointData
{
    public Vector2 Position { get; set; }
    public List<Vector2> ControlPoints { get; set; }
    public float WaitTime { get; set; }
    public float SpeedMultiplier { get; set; }
    public bool ShootingDisabled { get; set; }
}