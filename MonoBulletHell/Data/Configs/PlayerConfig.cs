using System;

namespace MonoBulletHell.Data.Configs;

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