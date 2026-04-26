using System;
using MonoBulletHell.Enums;

namespace MonoBulletHell.Data.Saves;

[Serializable]
public class SettingsSaveData
{
    public ScreenScale ScreenScale { get; set; }
    public float Volume { get; set; }
}