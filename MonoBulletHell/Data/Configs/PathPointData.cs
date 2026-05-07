using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoBulletHell.Data.Configs;

[Serializable]
public class PathPointData
{
    public Vector2 Position { get; set; }
    public List<Vector2> ControlPoints { get; set; }
    public float WaitTime { get; set; }
    public float SpeedMultiplier { get; set; }
    public bool ShootingDisabled { get; set; }

    public PathPointData Clone(Vector2 positionOverride)
    {
        return new PathPointData()
        {
            Position = positionOverride,
            ControlPoints = ControlPoints,
            WaitTime = WaitTime,
            SpeedMultiplier = SpeedMultiplier,
            ShootingDisabled = ShootingDisabled,
        };
    }
}