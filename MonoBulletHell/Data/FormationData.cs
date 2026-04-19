using System;

namespace MonoBulletHell.Data;

[Serializable]
public class FormationData
{
    public FormationType Type { get; set; }
    public int Count { get; set; }
    public float Rotation { get; set; }

    // Line
    public int Spacing { get; set; }

    // Circle
    public float Radius { get; set; }
    
    // Grid
    public int Rows { get; set; }
    public int Columns { get; set; }
    public float SpacingX { get; set; }
    public float SpacingY { get; set; }
}