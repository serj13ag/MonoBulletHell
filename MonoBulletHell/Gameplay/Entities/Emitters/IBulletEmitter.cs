namespace MonoBulletHell.Gameplay.Entities.Emitters;

public interface IBulletEmitter : IBaseEntity
{
    void SetShootingDisabled(bool pathBlockShootingDisabled);
    void Update(float timeServiceDeltaTime);
}