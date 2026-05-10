using System;
using Microsoft.Xna.Framework;

namespace MonoBulletHell.Gameplay.Movement;

public class StaticMovement : IMovement
{
    public Vector2 Position { get; }

    public bool IsFinished => false;
    public bool ShootingDisabled => false;

    public event Action<bool> ShootingDisabledChanged;

    public StaticMovement(Vector2 position)
    {
        Position = position;
    }

    public void Update(float deltaTime)
    {
    }
}