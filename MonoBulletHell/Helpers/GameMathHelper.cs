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

    public static Vector2 QuadraticBezier(Vector2 p0, Vector2 p1, Vector2 p2, float t)
    {
        return (1 - t) * (1 - t) * p0 +
               2 * (1 - t) * t * p1 +
               t * t * p2;
    }

    public static Vector2 CubicBezier(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        var u = 1 - t;
        return u * u * u * p0 +
               3 * u * u * t * p1 +
               3 * u * t * t * p2 +
               t * t * t * p3;
    }
}