using System;
using Microsoft.Xna.Framework;

namespace MonoBulletHell.Helpers;

public static class GameMathHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="baseRotation">Base rotation of the sprite. 0 - up</param>
    /// <returns></returns>
    public static float GetRotation(Vector2 direction, float baseRotation)
    {
        var angle = (float)Math.Atan2(direction.Y, direction.X);
        angle += MathHelper.PiOver2;

        var rotation = angle + MathHelper.ToRadians(baseRotation); // TODO: move base rotation to sprite
        return rotation;
    }
}