using Gum.Forms.DefaultVisuals.V3;
using Microsoft.Xna.Framework;
using MonoBulletHell.Helpers;

namespace MonoBulletHell;

public static class Constants
{
    public const int VirtualWidth = 320;
    public const int VirtualHeight = 480;

    public const int BulletDamage = 1;

    public static class Colors // TODO: config
    {
        public static readonly Color UiPanel = ColorHelper.FromHex("#0D170D");

        public static readonly Color SceneTitleBackground = ColorHelper.FromHex("#1B2F1B");
        public static readonly Color SceneGameplayBackground = ColorHelper.FromHex("#1B2F1B");
        public static readonly Color GameplayBackgroundTexture = ColorHelper.FromHex("#1B2F1B").Adjust(-20f);

        public static readonly Color UiButton = ColorHelper.FromHex("#3F5F3F");
        public static readonly Color PlayerShip = ColorHelper.FromHex("#3F5F3F");
        public static readonly Color Enemy = ColorHelper.FromHex("#3F5F3F");
        
        public static readonly Color PlayerBullet = ColorHelper.FromHex("#E8FFE8");
        public static readonly Color BulletImpact = ColorHelper.FromHex("#E8FFE8");
        
        public static readonly Color EnemyBullet = ColorHelper.FromHex("#c45c0c");
        public static readonly Color PlayerShipCore = ColorHelper.FromHex("#c45c0c");
    }
}