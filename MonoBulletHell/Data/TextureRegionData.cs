using System;

namespace MonoBulletHell.Data;

[Serializable]
public class TextureRegionData
{
    public string Name { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}