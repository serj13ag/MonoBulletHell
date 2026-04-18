using Microsoft.Xna.Framework;
using MonoBulletHell.Helpers;

namespace MonoBulletHell;

public static class Constants
{
    public const int VirtualWidth = 320;
    public const int VirtualHeight = 480;

    public static class Colors // TODO: config
    {
        public static readonly Color BackgroundColor = ColorHelper.FromHex("#5C4B51");
        public static readonly Color ShipColor = ColorHelper.FromHex("#8CBEB2");
        public static readonly Color ShipCoreColor = ColorHelper.FromHex("#F3B562");
        public static readonly Color EnemyColor = ColorHelper.FromHex("#F06060");
        public static readonly Color BeigeColor = ColorHelper.FromHex("#F2EBBF");
    }
}