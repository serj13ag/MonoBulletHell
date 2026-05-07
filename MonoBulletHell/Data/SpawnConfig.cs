using System;
using System.Collections.Generic;

namespace MonoBulletHell.Data;

[Serializable]
public class SpawnConfig
{
    public List<WaveData> Waves { get; set; }
    public BossData Boss { get; set; }
}