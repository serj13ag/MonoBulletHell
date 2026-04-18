using System;
using System.Collections.Generic;

namespace MonoBulletHell.Data;

[Serializable]
public class SpawnData
{
    public List<WaveData> Waves { get; set; }
}

[Serializable]
public class WaveData
{
    public float SpawnTime { get; set; }
    public string PathName { get; set; }
    public List<EnemyData> Enemies { get; set; }
}

[Serializable]
public class EnemyData
{
    public float NormalizedSpawnPositionX { get; set; } // TODO: remove?
    public float NormalizedSpawnPositionY { get; set; }
}