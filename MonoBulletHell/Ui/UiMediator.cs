using System.Collections.Generic;
using MonoBulletHell.Audio;
using MonoBulletHell.Screen;
using MonoBulletHell.Services;

namespace MonoBulletHell.Ui;

public interface IUiMediator
{
    IEnumerable<ScreenScale> GetScreenScales();
    int GetCurrentScaleIndex();
    void ResolutionScaleSelected(int scaleIndex);

    float GetCurrentVolume();
    void VolumeChanged(double sliderValue);

    void ButtonClicked();
}

public class UiMediator : IUiMediator
{
    private readonly ISettingsService _settingsService;
    private readonly ISoundService _soundService;

    public UiMediator(ISettingsService settingsService, ISoundService soundService)
    {
        _settingsService = settingsService;
        _soundService = soundService;
    }

    public IEnumerable<ScreenScale> GetScreenScales() => _settingsService.Scales;
    public int GetCurrentScaleIndex() => _settingsService.CurrentScaleIndex;
    public void ResolutionScaleSelected(int newScaleIndex) => _settingsService.SetScreenScaleByIndex(newScaleIndex);

    public float GetCurrentVolume() => _settingsService.Volume;
    public void VolumeChanged(double sliderValue) => _settingsService.SetVolume((float)sliderValue);

    public void ButtonClicked()
    {
        _soundService.PlaySoundEffect(SfxType.Click);
    }
}