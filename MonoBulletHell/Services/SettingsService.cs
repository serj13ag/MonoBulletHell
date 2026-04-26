using System;
using System.Collections.Generic;
using MonoBulletHell.Data.Saves;
using MonoBulletHell.Enums;

namespace MonoBulletHell.Services;

public interface ISettingsService
{
    ScreenScale ScreenScale { get; }
    IReadOnlyList<ScreenScale> Scales { get; }
    int CurrentScaleIndex { get; }

    float Volume { get; }

    event Action<ScreenScale> ScreenScaleChanged;
    event Action<float> VolumeChanged;

    void Initialize();

    void SetScreenScaleByIndex(int scaleIndex);
    void SetVolume(float value);
}

public class SettingsService : ISettingsService
{
    private const string SaveFileName = "settings";

    private readonly List<ScreenScale> _scales =
    [
        ScreenScale.X1,
        ScreenScale.X1_5,
        ScreenScale.X2,
        ScreenScale.X2_5,
        ScreenScale.X3,
    ];

    private readonly ISaveService _saveService;

    private ScreenScale _screenScale;
    private float _volume;

    public ScreenScale ScreenScale
    {
        get => _screenScale;
        private set
        {
            if (_screenScale != value)
            {
                _screenScale = value;
                ScreenScaleChanged?.Invoke(value);
            }
        }
    }

    public IReadOnlyList<ScreenScale> Scales => _scales;
    public int CurrentScaleIndex => _scales.IndexOf(ScreenScale);

    public float Volume
    {
        get => _volume;
        private set
        {
            if (_volume != value)
            {
                _volume = value;
                VolumeChanged?.Invoke(value);
            }
        }
    }

    public event Action<ScreenScale> ScreenScaleChanged;
    public event Action<float> VolumeChanged;

    public SettingsService(ISaveService saveService)
    {
        _saveService = saveService;
    }

    public void Initialize()
    {
        if (_saveService.TryLoad<SettingsSaveData>(SaveFileName, out var data))
        {
            _screenScale = data.ScreenScale;
            _volume = data.Volume;
        }
        else
        {
            _screenScale = ScreenScale.X2;
            _volume = 0.5f;
            Save();
        }
    }

    public void SetScreenScaleByIndex(int scaleIndex)
    {
        ScreenScale = _scales[scaleIndex];
        Save();
    }

    public void SetVolume(float value)
    {
        Volume = value;
        Save();
    }

    private void Save()
    {
        var settingsSave = new SettingsSaveData()
        {
            ScreenScale = _screenScale,
            Volume = _volume,
        };

        _saveService.Save(settingsSave, SaveFileName);
    }
}