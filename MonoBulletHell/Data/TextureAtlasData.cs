using System;
using System.Collections.Generic;

namespace MonoBulletHell.Data;

[Serializable]
public class TextureAtlasData
{
    public string Texture { get; set; }
    public List<TextureRegionData> Regions { get; set; }
}