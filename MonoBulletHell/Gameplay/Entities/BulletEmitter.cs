using Microsoft.Xna.Framework;
using MonoBulletHell.AnimatedValues;
using MonoBulletHell.Data;
using MonoBulletHell.Data.DTOs;
using MonoBulletHell.Gameplay.Services;
using MonoBulletHell.Helpers;

namespace MonoBulletHell.Gameplay.Entities;

public class BulletEmitter // TODO: organize folders
{
    private readonly IBulletService _bulletService;

    private readonly Vector2 _offset;
    private readonly IAnimatedFloat _roundsPerSecond;

    private readonly IAnimatedFloat _bulletSpeed;
    private readonly IAnimatedFloat _bulletAcceleration;
    private readonly IAnimatedFloat _bulletAngularVelocity;

    private readonly int _numberOfLines;
    private readonly IAnimatedFloat _angleBetweenLines;
    private readonly int _numberOfBulletsPerLine;
    private readonly IAnimatedFloat _angleBetweenBulletsInLine;

    private readonly IAnimatedFloat _spinPerSecond;

    private float _elapsedTime;
    private Vector2 _position;
    private bool _shootingDisabled;
    private float _timeTillShoot;

    private readonly IAnimatedFloat _angle;
    private float _spinAngle;

    public BulletEmitter(EmitterData emitterData, IBulletService bulletService)
    {
        _bulletService = bulletService;

        _offset = emitterData.Offset;
        _roundsPerSecond = emitterData.RoundsPerSecond;
        _angle = emitterData.StartingAngle;

        _bulletSpeed = emitterData.BulletSpeed;
        _bulletAcceleration = emitterData.BulletAcceleration;
        _bulletAngularVelocity = emitterData.BulletAngularVelocity;

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
        _elapsedTime += deltaTime;
        _spinAngle += _spinPerSecond.Evaluate(_elapsedTime) * deltaTime;

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

            var shotCooldown = 1 / _roundsPerSecond.Evaluate(_elapsedTime);
            _timeTillShoot += shotCooldown;
        }
        else
        {
            _timeTillShoot -= deltaTime;
        }
    }

    private void Shoot()
    {
        var totalAngle = _angle.Evaluate(_elapsedTime) + _spinAngle;

        for (var line = 0; line < _numberOfLines; line++)
        {
            var lineAngle = totalAngle + line * _angleBetweenLines.Evaluate(_elapsedTime);

            for (var bullet = 0; bullet < _numberOfBulletsPerLine; bullet++)
            {
                var bulletAngle = lineAngle + bullet * _angleBetweenBulletsInLine.Evaluate(_elapsedTime);

                SpawnBullet(bulletAngle);
            }
        }
    }

    private void SpawnBullet(float angle)
    {
        var bulletDto = new BulletDTO
        {
            Position = _position + _offset,
            Direction = GameMathHelper.DegreeToDirection(angle),
            Speed = _bulletSpeed.Evaluate(_elapsedTime),
            Damage = Constants.BulletDamage,
            IsPlayer = false,
            Acceleration = _bulletAcceleration.Evaluate(_elapsedTime),
            AngularVelocity = _bulletAngularVelocity.Evaluate(_elapsedTime),
        };

        _bulletService.SpawnBullet(bulletDto);
    }
}