using System;
using System.Collections.Generic;

namespace MonoBulletHell.Data.Configs;

[Serializable]
public class PathData
{
    public string Name { get; set; }
    public float Speed { get; set; }
    public bool InfinitelyLooped { get; set; }
    public int Loops { get; set; }
    public PathType Type { get; set; }
    public List<PathPointData> Points { get; set; }
}