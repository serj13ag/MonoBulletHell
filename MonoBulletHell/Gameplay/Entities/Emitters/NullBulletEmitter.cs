using Microsoft.Xna.Framework;

namespace MonoBulletHell.Gameplay.Entities.Emitters;

public class NullBulletEmitter : IBulletEmitter
{
    public static readonly NullBulletEmitter Instance = new NullBulletEmitter();

    public Vector2 Position { get; set; }
    public float Rotation { get; set; }

    public void SetShootingDisabled(bool pathBlockShootingDisabled)
    {
    }

    public void Update(float timeServiceDeltaTime)
    {
    }
}