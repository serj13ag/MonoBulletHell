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

    /// <summary>
    /// 0 => right, 90 => up, 180 => left, 270 => down
    /// </summary>
    /// <param name="degree"></param>
    /// <returns></returns>
    public static Vector2 DegreeToDirection(float degree)
    {
        var radian = degree * (MathF.PI / 180f);

        var x = MathF.Cos(radian);
        var y = -MathF.Sin(radian); // because Y is inverted

        return new Vector2(x, y);
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