using Microsoft.Xna.Framework;

namespace MonoBulletHell.Gameplay.Entities;

public interface IBaseEntity
{
    Vector2 Position { get; set; }
    float Rotation { get; set; }
}