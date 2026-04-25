using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoBulletHell.Data;

[Serializable]
public class EnemyConfig
{
    public List<EnemyData> Enemies { get; set; }
}

[Serializable]
public class EnemyData
{
    public string Name { get; set; }
    public string SpriteName { get; set; }
    public int Health { get; set; }
    public float Speed { get; set; }
    public Vector2 ColliderOffset { get; set; }
    public float ColliderRadius { get; set; }
    public float ShootCooldown { get; set; }
    public float BulletSpeed { get; set; }
}