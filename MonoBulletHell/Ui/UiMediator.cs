using System.Collections.Generic;
using MonoBulletHell.Enums;
using MonoBulletHell.Services;

namespace MonoBulletHell.Ui;

public interface IUiMediator
{
    IEnumerable<ScreenScale> GetScreenScales();
    int GetCurrentScaleIndex();
    void ResolutionScaleSelected(int scaleIndex);
}

public class UiMediator : IUiMediator
{
    private readonly ISettingsService _settingsService;

    public UiMediator(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    public IEnumerable<ScreenScale> GetScreenScales() => _settingsService.Scales;
    public int GetCurrentScaleIndex() => _settingsService.CurrentScaleIndex;
    public void ResolutionScaleSelected(int newScaleIndex) => _settingsService.SetScreenScaleByIndex(newScaleIndex);
}