using System;

namespace MonoBulletHell.Data.Configs;

[Serializable]
public class FormationData
{
    public FormationType Type { get; set; }
    public float Rotation { get; set; }

    public int Count { get; set; } // Line, Circle
    public int Spacing { get; set; } // Line
    public float Radius { get; set; } // Circle

    public int Rows { get; set; } // Grid, VShape
    public int Columns { get; set; } // Grid
    public float SpacingX { get; set; } // Grid, VShape
    public float SpacingY { get; set; } // Grid, VShape
    public bool Inverted { get; set; } // VShape
}