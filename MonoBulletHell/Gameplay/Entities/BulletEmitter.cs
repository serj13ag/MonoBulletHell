using Microsoft.Xna.Framework;
using MonoBulletHell.Data;
using MonoBulletHell.Gameplay.Services;
using MonoBulletHell.Helpers;

namespace MonoBulletHell.Gameplay.Entities;

public class BulletEmitter // TODO: organize folders
{
    private readonly IBulletService _bulletService;

    private readonly EmitterData _emitterData;

    private readonly Vector2 _offset;
    private readonly float _shotCooldown;
    private readonly float _bulletSpeed;
    private readonly float _angle;

    private Vector2 _position;
    private bool _shootingDisabled;
    private float _timeTillShoot;

    public BulletEmitter(EmitterData emitterData, IBulletService bulletService)
    {
        _bulletService = bulletService;

        _emitterData = emitterData;

        _offset = emitterData.Offset;
        _shotCooldown = 1 / emitterData.RoundsPerSecond;
        _bulletSpeed = emitterData.BulletSpeed;
        _angle = emitterData.StartingAngle;
    }

    public void SetPosition(Vector2 position) => _position = position;
    public void SetShootingDisabled(bool value) => _shootingDisabled = value;

    public void Update(float deltaTime)
    {
        HandleShooting(deltaTime);
    }

    private void HandleShooting(float deltaTime)
    {
        if (_timeTillShoot <= 0f)
        {
            if (!_shootingDisabled)
            {
                Shoot();
            }

            _timeTillShoot += _shotCooldown;
        }
        else
        {
            _timeTillShoot -= deltaTime;
        }
    }

    private void Shoot()
    {
        for (var i = 0; i < _emitterData.NumberOfLines; i++)
        {
            var bulletAngle = _angle + _emitterData.AngleBetweenLines * i;
            SpawnBullet(bulletAngle);
        }
    }

    private void SpawnBullet(float angle)
    {
        _bulletService.SpawnBullet(_position + _offset, GameMathHelper.DegreeToDirection(angle), _bulletSpeed,
            Constants.BulletDamage, false);
    }
}