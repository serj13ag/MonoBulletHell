using System;
using Microsoft.Xna.Framework;

namespace MonoBulletHell.Gameplay.Entities.PathBlocks;

public interface IPathBlock
{
    Vector2 Position { get; }

    bool IsFinished { get; }
    bool ShootingDisabled { get; }

    event Action<bool> ShootingDisabledChanged;

    void Update(float deltaTime);
}