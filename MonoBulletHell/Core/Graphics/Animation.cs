using System;
using System.Collections.Generic;

namespace MonoBulletHell.Core.Graphics;

public class Animation
{
    public List<TextureRegion> Frames { get; }
    public TimeSpan Delay { get; }

    public Animation(List<TextureRegion> frames, int fps)
    {
        Frames = frames;
        Delay = TimeSpan.FromSeconds(1.0 / fps);
    }
}