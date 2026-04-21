using System;

namespace MonoBulletHell.Data;

[Serializable]
public class PlayerConfig
{
    public int Health { get; set; }
    public float Speed { get; set; }
    public float ShootCooldown { get; set; }
    public float BulletSpeed { get; set; }
    public float DamageImmuneCooldown { get; set; }

    public string SpriteName { get; set; }
}