using System;
using System.Collections.Generic;

namespace MonoBulletHell.Data.Configs;

[Serializable]
public class PathData
{
    public string Name { get; set; }
    public float Speed { get; set; }
    public int Loops { get; set; } // TODO: add infinite loop
    public PathType Type { get; set; }
    public List<PathPointData> Points { get; set; }
    // TODO: add inverse bool
}