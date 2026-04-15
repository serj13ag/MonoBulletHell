using System;
using System.Collections.Generic;

namespace MonoBulletHell.Core.Graphics.Data;

[Serializable]
public class AnimationData
{
    public string Name { get; set; }
    public int Fps { get; set; }

    /// <summary>
    /// Region names
    /// </summary>
    public List<string> Frames { get; set; }
}