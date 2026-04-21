using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoBulletHell.Data;

[Serializable]
public class SpawnConfig
{
    public List<WaveData> Waves { get; set; }
}

[Serializable]
public class WaveData
{
    public float SpawnTime { get; set; }
    public Vector2 Position { get; set; }
    public string PathName { get; set; }
    public string EnemyName { get; set; }
    public FormationData Formation { get; set; }
}