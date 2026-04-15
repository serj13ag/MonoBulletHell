using System;
using Microsoft.Xna.Framework;

namespace MonoBulletHell.Core.Graphics;

public class AnimatedSprite : Sprite
{
    private readonly Animation _animation;

    private int _currentFrame;
    private TimeSpan _elapsed;

    public AnimatedSprite(Animation animation)
        : base(animation.Frames[0])
    {
        _animation = animation;
    }

    public void Update(GameTime gameTime)
    {
        _elapsed += gameTime.ElapsedGameTime;

        if (_elapsed < _animation.Delay)
        {
            return;
        }

        _elapsed -= _animation.Delay;
        _currentFrame++;

        if (_currentFrame >= _animation.Frames.Count)
        {
            _currentFrame = 0;
        }

        SetRegion(_animation.Frames[_currentFrame]);
    }
}