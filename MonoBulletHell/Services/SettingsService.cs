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

    event Action<ScreenScale> ScreenScaleChanged;

    void Initialize();

    void SetScreenScaleByIndex(int scaleIndex);
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

    public event Action<ScreenScale> ScreenScaleChanged;

    public SettingsService(ISaveService saveService)
    {
        _saveService = saveService;
    }

    public void Initialize()
    {
        if (_saveService.TryLoad<SettingsSaveData>(SaveFileName, out var data))
        {
            _screenScale = data.ScreenScale;
        }
        else
        {
            _screenScale = ScreenScale.X2;
            Save();
        }
    }

    public void SetScreenScaleByIndex(int scaleIndex)
    {
        ScreenScale = _scales[scaleIndex];
        Save();
    }

    private void Save()
    {
        var settingsSave = new SettingsSaveData()
        {
            ScreenScale = _screenScale,
        };

        _saveService.Save(settingsSave, SaveFileName);
    }
}