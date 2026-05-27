using System;

namespace MonoBulletHell.Data.Configs;

[Serializable]
public class GameConfig
{
    public PlayerConfig Player { get; set; }
    public ColorConfig Colors { get; set; }
    public string[] LevelConfigPaths { get; set; }
}