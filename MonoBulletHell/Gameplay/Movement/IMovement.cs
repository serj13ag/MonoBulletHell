using System;
using Microsoft.Xna.Framework;

namespace MonoBulletHell.Gameplay.Movement;

public interface IMovement
{
    Vector2 Position { get; }

    bool IsFinished { get; }
    bool ShootingDisabled { get; }

    event Action<bool> ShootingDisabledChanged;

    void Update(float deltaTime);
}