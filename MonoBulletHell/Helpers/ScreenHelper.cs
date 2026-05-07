using Microsoft.Xna.Framework;

namespace MonoBulletHell.Helpers;

public static class ScreenHelper
{
    public static Vector2 GetLerpScreenVirtualPosition(float x, float y)
    {
        var virtualWidth = MathHelper.Lerp(0, MonoBulletHellGame.VirtualWidth, x);
        var virtualHeight = MathHelper.Lerp(0, MonoBulletHellGame.VirtualHeight, y);
        return new Vector2(virtualWidth, virtualHeight);
    }

    public static Vector2 ToVirtualPosition(Vector2 normalized)
    {
        var pixelX = normalized.X * MonoBulletHellGame.VirtualWidth;
        var pixelY = normalized.Y * MonoBulletHellGame.VirtualHeight;
        return new Vector2(pixelX, pixelY);
    }

    public static bool IsOutOfVirtualBounds(Vector2 position)
    {
        return position.X < 0 || position.Y < 0 || position.X > MonoBulletHellGame.VirtualWidth || position.Y > MonoBulletHellGame.VirtualHeight;
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

        if (position.X > MonoBulletHellGame.VirtualWidth)
        {
            position.X = MonoBulletHellGame.VirtualWidth;
        }

        if (position.Y > MonoBulletHellGame.VirtualHeight)
        {
            position.Y = MonoBulletHellGame.VirtualHeight;
        }
    }
}