using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoBulletHell.Data;

[Serializable]
public class SpawnConfig
{
    public List<WaveData> Waves { get; set; }
    public BossData Boss { get; set; }
}

[Serializable]
public class WaveData
{
    public float SpawnTime { get; set; }
    public Vector2 Position { get; set; }
    public string PathName { get; set; }
    public string EnemyName { get; set; }
    public string EmitterName { get; set; }
    public FormationData Formation { get; set; }
}

[Serializable]
public class BossData
{
    public string EnemyName { get; set; }
    public Vector2 Position { get; set; }
    public List<BossStageData> Stages { get; set; }
}

[Serializable]
public class BossStageData
{
    public float HealthPercent { get; set; }
    public string PathName { get; set; }
    public string EmitterName { get; set; }
}