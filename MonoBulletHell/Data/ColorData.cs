using System;
using Microsoft.Xna.Framework;

namespace MonoBulletHell.Data;

[Serializable]
public class ColorData
{
    public string Hex { get; set; }

    public static implicit operator Color(ColorData data)
    {
        var hex = data.Hex.Replace("#", "");
        var r = Convert.ToInt32(hex.Substring(0, 2), 16);
        var g = Convert.ToInt32(hex.Substring(2, 2), 16);
        var b = Convert.ToInt32(hex.Substring(4, 2), 16);
        return new Color(r, g, b);
    }
}