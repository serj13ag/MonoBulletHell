using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoBulletHell.Data.Configs;

[Serializable]
public class BossData
{
    public string EnemyName { get; set; }
    public Vector2 Position { get; set; }
    public List<BossStageData> Stages { get; set; }
}