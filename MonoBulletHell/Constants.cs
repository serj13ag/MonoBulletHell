using Microsoft.Xna.Framework.Graphics;

namespace MonoBulletHell;

public static class Constants
{
    public static readonly SamplerState SamplerState = SamplerState.PointClamp;

    public const int VirtualWidth = 320;
    public const int VirtualHeight = 480;

    public const int ActualWidth = (int)(VirtualWidth * ScreenScale);
    public const int ActualHeight = (int)(VirtualHeight * ScreenScale);

    private const float ScreenScale = 1f;
}