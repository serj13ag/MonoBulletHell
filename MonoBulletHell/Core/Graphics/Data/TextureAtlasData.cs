using System;
using System.Collections.Generic;

namespace MonoBulletHell.Core.Graphics.Data;

[Serializable]
public class TextureAtlasData
{
    public string Texture { get; set; }
    public List<TextureRegionData> Regions { get; set; }
    public List<AnimationData> Animations { get; set; }
}