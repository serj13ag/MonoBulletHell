using Microsoft.Xna.Framework;

namespace MonoBulletHell.Helpers;

public static class ScreenHelper
{
    public static Vector2 GetLerpScreenVirtualPosition(float x, float y)
    {
        var virtualWidth = MathHelper.Lerp(0, Constants.VirtualWidth, x);
        var virtualHeight = MathHelper.Lerp(0, Constants.VirtualHeight, y);
        return new Vector2(virtualWidth, virtualHeight);
    }

    public static bool IsOutOfVirtualBounds(Vector2 position)
    {
        return position.X < 0 || position.Y < 0 || position.X > Constants.VirtualWidth || position.Y > Constants.VirtualHeight;
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

        if (position.X > Constants.VirtualWidth)
        {
            position.X = Constants.VirtualWidth;
        }

        if (position.Y > Constants.VirtualHeight)
        {
            position.Y = Constants.VirtualHeight;
        }
    }
}