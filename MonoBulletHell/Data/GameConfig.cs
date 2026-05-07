using System;

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
    public string UiPanel { get; set; }
    public string UiButton { get; set; }

    public string TitleSceneBackground { get; set; }
    public string GameplaySceneBackground { get; set; }
    public string GameplayBackgroundTexture { get; set; }

    public string PlayerShip { get; set; }
    public string PlayerShipCore { get; set; }

    public string Enemy { get; set; }

    public string PlayerBullet { get; set; }
    public string EnemyBullet { get; set; }
    public string BulletImpact { get; set; }
}