using System;
using Microsoft.Xna.Framework;

namespace MonoBulletHell.Data;

[Serializable]
public class GameConfig
{
    public PlayerConfig Player { get; set; }
    public ColorConfig Colors { get; set; }
}

[Serializable]
public class PlayerConfig
{
    public int Health { get; set; }
    public float Speed { get; set; }
    public float FocusSpeed { get; set; }
    public float ShootCooldown { get; set; }
    public float BulletSpeed { get; set; }
    public float DamageImmuneCooldown { get; set; }

    public string SpriteName { get; set; }
    public string CoreSpriteName { get; set; }
}

[Serializable]
public class ColorConfig
{
    public ColorData UiPanel { get; set; }
    public ColorData UiButton { get; set; }

    public ColorData TitleSceneBackground { get; set; }
    public ColorData GameplaySceneBackground { get; set; }
    public ColorData GameplayBackgroundTexture { get; set; }

    public ColorData PlayerShip { get; set; }
    public ColorData PlayerShipCore { get; set; }

    public ColorData Enemy { get; set; }

    public ColorData PlayerBullet { get; set; }
    public ColorData EnemyBullet { get; set; }
    public ColorData BulletImpact { get; set; }
}

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