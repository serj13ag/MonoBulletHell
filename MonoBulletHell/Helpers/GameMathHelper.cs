using System;
using Microsoft.Xna.Framework;

namespace MonoBulletHell.Helpers;

public static class GameMathHelper
{
    /// <summary>
    /// Calculate rotation in radians
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static float GetRotation(Vector2 direction)
    {
        var angle = (float)Math.Atan2(direction.Y, direction.X);
        return angle + MathHelper.PiOver2;
    }
}