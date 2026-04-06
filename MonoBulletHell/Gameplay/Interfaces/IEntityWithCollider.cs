using MonoBulletHell.Core;

namespace MonoBulletHell.Gameplay.Interfaces;

public interface IEntityWithCollider
{
    public Circle Collider { get; }
}