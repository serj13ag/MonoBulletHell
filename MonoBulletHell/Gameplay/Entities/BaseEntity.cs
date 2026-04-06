using Microsoft.Xna.Framework;

namespace MonoBulletHell.Gameplay.Entities;

public abstract class BaseEntity
{
    public Vector2 Position { get; set; }

    /// <summary>
    /// Rotation in radians
    /// </summary>
    public float Rotation { get; set; }
}