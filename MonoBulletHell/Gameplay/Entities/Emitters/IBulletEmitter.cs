namespace MonoBulletHell.Gameplay.Entities.Emitters;

public interface IBulletEmitter : IBaseEntity
{
    void SetShootingDisabled(bool shootingDisabled);
    void Update(float timeServiceDeltaTime);
}