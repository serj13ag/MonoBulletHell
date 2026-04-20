using Microsoft.Xna.Framework;
using MonoBulletHell.Helpers;

namespace MonoBulletHell;

public static class Constants
{
    public const int VirtualWidth = 320;
    public const int VirtualHeight = 480;

    public static class Colors // TODO: config
    {
        public static readonly Color BackgroundDark = ColorHelper.FromHex("#0D170D");
        public static readonly Color BackgroundColor = ColorHelper.FromHex("#1B2F1B");
        public static readonly Color BackgroundHighlight = ColorHelper.FromHex("#3F5F3F");
        public static readonly Color ShipColor = ColorHelper.FromHex("#BFE8BF");
        public static readonly Color ShipProjectile = ColorHelper.FromHex("#E8FFE8");
        public static readonly Color EnemyColor = ColorHelper.FromHex("#6A8F6A");
        public static readonly Color EnemyProjectiles = ColorHelper.FromHex("#c45c0c");
    }
}