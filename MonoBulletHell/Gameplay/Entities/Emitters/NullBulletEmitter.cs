using Microsoft.Xna.Framework;

namespace MonoBulletHell.Gameplay.Entities.Emitters;

public class NullBulletEmitter : IBulletEmitter
{
    public static readonly NullBulletEmitter Instance = new NullBulletEmitter();

    public void SetPosition(Vector2 position)
    {
    }

    public void SetShootingDisabled(bool pathBlockShootingDisabled)
    {
    }

    public void Update(float timeServiceDeltaTime)
    {
    }
}