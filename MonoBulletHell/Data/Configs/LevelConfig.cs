using System;
using System.Collections.Generic;

namespace MonoBulletHell.Data.Configs;

[Serializable]
public class LevelConfig
{
    public List<WaveData> Waves { get; set; }
    public BossData Boss { get; set; }
}