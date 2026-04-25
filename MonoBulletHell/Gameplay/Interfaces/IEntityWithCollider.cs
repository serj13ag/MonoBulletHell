using MonoBulletHell.Gameplay.Entities;

namespace MonoBulletHell.Gameplay.Interfaces;

public interface IEntityWithCollider
{
    public CircleCollider Collider { get; }
}