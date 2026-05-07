using System;
using Microsoft.Xna.Framework;

namespace MonoBulletHell.Data;

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