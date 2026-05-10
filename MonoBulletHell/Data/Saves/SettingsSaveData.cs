using System;
using MonoBulletHell.Screen;

namespace MonoBulletHell.Data.Saves;

[Serializable]
public class SettingsSaveData
{
    public ScreenScale ScreenScale { get; set; }
    public float Volume { get; set; }
}