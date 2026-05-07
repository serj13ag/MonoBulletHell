using System;

namespace MonoBulletHell.Data.Configs;

[Serializable]
public class BossStageData
{
    public float HealthPercent { get; set; }
    public string PathName { get; set; }
    public string EmitterName { get; set; }
}