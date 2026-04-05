using Microsoft.Xna.Framework;

namespace MonoBulletHell.Helpers;

public static class ScreenHelper
{
    public static bool IsOutOfVirtualBounds(Vector2 position)
    {
        return position.X < 0 || position.Y < 0 || position.X > Constants.VirtualWidth || position.Y > Constants.VirtualHeight;
    }
}