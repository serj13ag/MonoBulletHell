using System;
using Microsoft.Xna.Framework;

namespace MonoBulletHell.Helpers;

public static class ColorHelper
{
    public static Color FromHex(string hex)
    {
        hex = hex.Replace("#", "");
        var r = Convert.ToInt32(hex.Substring(0, 2), 16);
        var g = Convert.ToInt32(hex.Substring(2, 2), 16);
        var b = Convert.ToInt32(hex.Substring(4, 2), 16);
        return new Color(r, g, b);
    }
}