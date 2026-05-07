using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoBulletHell.Data.Configs;
using MonoBulletHell.Helpers;

namespace MonoBulletHell.Gameplay.Entities.PathBlocks;

public class PathBlock : IPathBlock
{
    private readonly List<PathPointData> _pathPoints;
    private readonly float _speed;

    private bool _movingToFirstPoint;
    private int _currentIndex;
    private float _waitTimer;
    private bool _isWaiting;

    private Vector2 _currentPosition;
    private Vector2 _startPosition;
    private Vector2 _targetPosition;

    private int _loopsLeft;
    private float _progress;
    private bool _shootingDisabled;

    public bool ShootingDisabled
    {
        get => _shootingDisabled;
        private set
        {
            if (_shootingDisabled != value)
            {
                _shootingDisabled = value;
                ShootingDisabledChanged?.Invoke(value);
            }
        }
    }

    public bool IsFinished { get; private set; }

    public Vector2 Position => _currentPosition;

    public event Action<bool> ShootingDisabledChanged;

    public PathBlock(float speed, int loops, List<PathPointData> pathPoints, Vector2? startPosition = null)
    {
        _pathPoints = pathPoints;
        _speed = speed;

        _loopsLeft = loops;
        _currentIndex = 0;

        if (startPosition.HasValue)
        {
            _movingToFirstPoint = true;
            UpdatePositions(startPosition.Value, pathPoints[0].Position);
        }
        else
        {
            UpdatePositions(pathPoints[0].Position, pathPoints[1].Position);
        }

        _currentPosition = _startPosition;

        _shootingDisabled = _pathPoints[0].ShootingDisabled;
    }

    public void Update(float deltaTime)
    {
        if (IsFinished)
        {
            return;
        }

        if (_isWaiting)
        {
            UpdateWaiting(deltaTime);
        }
        else
        {
            UpdateMoving(deltaTime);
        }
    }

    private void UpdateMoving(float deltaTime)
    {
        var moveSpeed = _speed;

        if (_pathPoints[_currentIndex].SpeedMultiplier > 0f)
        {
            moveSpeed *= _pathPoints[_currentIndex].SpeedMultiplier;
        }

        var distance = Vector2.Distance(_startPosition, _targetPosition);
        if (distance <= 0.0001f)
        {
            distance = 0.0001f;
        }

        var deltaProgress = (moveSpeed * deltaTime) / distance;
        _progress += deltaProgress;

        if (_progress >= 1f)
        {
            _currentPosition = _targetPosition;
            OnReachPoint();
        }
        else
        {
            var nextPoint = _pathPoints[_currentIndex + 1];
            _currentPosition = CalculateCurrentPosition(nextPoint);
        }
    }

    private void UpdateWaiting(float deltaTime)
    {
        _waitTimer -= deltaTime;

        if (_waitTimer <= 0f)
        {
            _isWaiting = false;
            MoveToNextSegment();
        }
    }

    private void OnReachPoint()
    {
        if (_movingToFirstPoint)
        {
            _movingToFirstPoint = false;
            UpdatePositions(_pathPoints[0].Position, _pathPoints[1].Position);
            return;
        }

        _currentIndex++;

        var point = _pathPoints[_currentIndex];

        ShootingDisabled = point.ShootingDisabled;

        if (point.WaitTime > 0f)
        {
            _isWaiting = true;
            _waitTimer = point.WaitTime;
        }
        else
        {
            MoveToNextSegment();
        }
    }

    private void MoveToNextSegment()
    {
        if (_currentIndex >= _pathPoints.Count - 1)
        {
            HandleEndOfPath();
            return;
        }

        UpdatePositions(_pathPoints[_currentIndex].Position, _pathPoints[_currentIndex + 1].Position);
    }

    private void HandleEndOfPath()
    {
        if (_loopsLeft > 0)
        {
            _loopsLeft--;
            _currentIndex = 0;

            ShootingDisabled = _pathPoints[0].ShootingDisabled;
            UpdatePositions(_pathPoints[0].Position, _pathPoints[1].Position);
        }
        else
        {
            IsFinished = true;
        }
    }

    private Vector2 CalculateCurrentPosition(PathPointData nextPoint)
    {
        if (_movingToFirstPoint || nextPoint.ControlPoints == null)
        {
            return Vector2.Lerp(_startPosition, _targetPosition, _progress);
        }

        switch (nextPoint.ControlPoints.Count)
        {
            case 1:
            {
                var c = nextPoint.ControlPoints[0];
                return GameMathHelper.QuadraticBezier(_startPosition, c, _targetPosition, _progress);
            }
            case 2:
            {
                var c1 = nextPoint.ControlPoints[0];
                var c2 = nextPoint.ControlPoints[1];
                return GameMathHelper.CubicBezier(_startPosition, c1, c2, _targetPosition, _progress);
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void UpdatePositions(Vector2 start, Vector2 target)
    {
        _startPosition = start;
        _targetPosition = target;
        _progress = 0f;
    }
}