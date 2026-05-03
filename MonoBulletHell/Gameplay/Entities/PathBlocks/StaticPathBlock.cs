using System;
using Microsoft.Xna.Framework;

namespace MonoBulletHell.Gameplay.Entities.PathBlocks;

public class StaticPathBlock : IPathBlock
{
    public Vector2 Position { get; }

    public bool IsFinished => false;
    public bool ShootingDisabled => false;

    public event Action<bool> ShootingDisabledChanged;

    public StaticPathBlock(Vector2 position)
    {
        Position = position;
    }

    public void Update(float deltaTime)
    {
    }
}