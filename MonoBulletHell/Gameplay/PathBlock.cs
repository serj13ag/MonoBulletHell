using Microsoft.Xna.Framework;
using MonoBulletHell.Data;
using MonoBulletHell.Helpers;

namespace MonoBulletHell.Gameplay;

public class PathBlock
{
    private readonly PathData _path;
    private readonly float _speed;

    private int _currentIndex;
    private float _waitTimer;
    private bool _isWaiting;

    private Vector2 _currentPosition;
    private Vector2 _startPosition;
    private Vector2 _targetPosition;

    private int _loopsLeft;
    private float _progress;

    public bool ShootingDisabled { get; private set; }
    public bool IsFinished { get; private set; }

    public Vector2 Position => _currentPosition;

    public PathBlock(PathData path, float speed)
    {
        _path = path;
        _speed = speed;

        _loopsLeft = path.Loops;
        _currentIndex = 0;

        UpdatePositions(path.Points[0].Position, path.Points[1].Position);
        _currentPosition = _startPosition;

        ShootingDisabled = _path.Points[0].ShootingDisabled;
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

        if (_path.Points[_currentIndex].SpeedMultiplier > 0f)
        {
            moveSpeed *= _path.Points[_currentIndex].SpeedMultiplier;
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
            _currentPosition = Vector2.Lerp(_startPosition, _targetPosition, _progress);
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
        _currentIndex++;

        var point = _path.Points[_currentIndex];

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
        if (_currentIndex >= _path.Points.Count - 1)
        {
            HandleEndOfPath();
            return;
        }

        UpdatePositions(_path.Points[_currentIndex].Position, _path.Points[_currentIndex + 1].Position);
    }

    private void HandleEndOfPath()
    {
        if (_loopsLeft > 0)
        {
            _loopsLeft--;
            _currentIndex = 0;

            ShootingDisabled = _path.Points[0].ShootingDisabled;
            UpdatePositions(_path.Points[0].Position, _path.Points[1].Position);
        }
        else
        {
            IsFinished = true;
        }
    }

    private void UpdatePositions(Vector2 start, Vector2 target)
    {
        _startPosition = ScreenHelper.ToVirtualPosition(start);
        _targetPosition = ScreenHelper.ToVirtualPosition(target);
        _progress = 0f;
    }
}