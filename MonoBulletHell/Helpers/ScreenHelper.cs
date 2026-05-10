using Microsoft.Xna.Framework;
using MonoBulletHell.App;

namespace MonoBulletHell.Helpers;

public static class ScreenHelper
{
    public static Vector2 GetLerpScreenVirtualPosition(float x, float y)
    {
        var virtualWidth = MathHelper.Lerp(0, GameConstants.VirtualWidth, x);
        var virtualHeight = MathHelper.Lerp(0, GameConstants.VirtualHeight, y);
        return new Vector2(virtualWidth, virtualHeight);
    }

    public static Vector2 ToVirtualPosition(Vector2 normalized)
    {
        var pixelX = normalized.X * GameConstants.VirtualWidth;
        var pixelY = normalized.Y * GameConstants.VirtualHeight;
        return new Vector2(pixelX, pixelY);
    }

    public static bool IsOutOfVirtualBounds(Vector2 position)
    {
        return position.X < 0 || position.Y < 0 || position.X > GameConstants.VirtualWidth || position.Y > GameConstants.VirtualHeight;
    }

    public static void ClampToVirtualBounds(ref Vector2 position)
    {
        if (position.X < 0)
        {
            position.X = 0;
        }

        if (position.Y < 0)
        {
            position.Y = 0;
        }

        if (position.X > GameConstants.VirtualWidth)
        {
            position.X = GameConstants.VirtualWidth;
        }

        if (position.Y > GameConstants.VirtualHeight)
        {
            position.Y = GameConstants.VirtualHeight;
        }
    }
}