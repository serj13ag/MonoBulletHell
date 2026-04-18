using System;
using System.Collections.Generic;

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
}

[Serializable]
public class PathPointData
{
    public float X { get; set; }
    public float Y { get; set; }
    public float WaitTime { get; set; }
    public float SpeedMultiplier { get; set; }
    public bool ShootingDisabled { get; set; }
}