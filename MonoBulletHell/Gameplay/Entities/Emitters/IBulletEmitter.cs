using Microsoft.Xna.Framework;

namespace MonoBulletHell.Gameplay.Entities.Emitters;

public interface IBulletEmitter
{
    void SetPosition(Vector2 position);
    void SetShootingDisabled(bool pathBlockShootingDisabled);
    void Update(float timeServiceDeltaTime);
}