using System;

namespace MonoBulletHell.Data;

[Serializable]
public class GameConfig
{
    public PlayerConfig Player { get; set; }
    public ColorConfig Colors { get; set; }
}