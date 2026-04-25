using Microsoft.Xna.Framework;
using MonoBulletHell.Data;
using MonoBulletHell.Gameplay.Services;
using MonoBulletHell.Helpers;

namespace MonoBulletHell.Gameplay.Entities;

public class BulletEmitter // TODO: organize folders
{
    private readonly IBulletService _bulletService;

    private readonly Vector2 _offset;
    private readonly float _shotCooldown;
    private readonly float _bulletSpeed;

    private readonly int _numberOfLines;
    private readonly float _angleBetweenLines;
    private readonly int _numberOfBulletsPerLine;
    private readonly float _angleBetweenBulletsInLine;

    private readonly float _spinPerSecond;

    private Vector2 _position;
    private bool _shootingDisabled;
    private float _timeTillShoot;
    private float _angle;

    public BulletEmitter(EmitterData emitterData, IBulletService bulletService)
    {
        _bulletService = bulletService;

        _offset = emitterData.Offset;
        _shotCooldown = 1 / emitterData.RoundsPerSecond;
        _bulletSpeed = emitterData.BulletSpeed;
        _angle = emitterData.StartingAngle;

        _numberOfLines = emitterData.NumberOfLines == 0 ? 1 : emitterData.NumberOfLines;
        _angleBetweenLines = emitterData.AngleBetweenLines;
        _numberOfBulletsPerLine = emitterData.NumberOfBulletsPerLine == 0 ? 1 : emitterData.NumberOfBulletsPerLine;
        _angleBetweenBulletsInLine = emitterData.AngleBetweenBulletsInLine;

        _spinPerSecond = emitterData.SpinPerSecond;
    }

    public void SetPosition(Vector2 position) => _position = position;
    public void SetShootingDisabled(bool value) => _shootingDisabled = value;

    public void Update(float deltaTime)
    {
        UpdateAngle(deltaTime);
        HandleShooting(deltaTime);
    }

    private void UpdateAngle(float deltaTime)
    {
        _angle += _spinPerSecond * deltaTime;
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
        for (var line = 0; line < _numberOfLines; line++)
        {
            var lineAngle = _angle + line * _angleBetweenLines;

            for (var bullet = 0; bullet < _numberOfBulletsPerLine; bullet++)
            {
                var bulletAngle = lineAngle + bullet * _angleBetweenBulletsInLine;

                SpawnBullet(bulletAngle);
            }
        }
    }

    private void SpawnBullet(float angle)
    {
        _bulletService.SpawnBullet(_position + _offset, GameMathHelper.DegreeToDirection(angle), _bulletSpeed,
            Constants.BulletDamage, false);
    }
}