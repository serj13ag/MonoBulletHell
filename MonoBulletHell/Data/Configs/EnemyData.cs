using System;
using Microsoft.Xna.Framework;

namespace MonoBulletHell.Data.Configs;

[Serializable]
public class EnemyData
{
    public string Name { get; set; }
    public string SpriteName { get; set; }
    public int Health { get; set; }
    public Vector2 ColliderOffset { get; set; }
    public float ColliderRadius { get; set; }
}