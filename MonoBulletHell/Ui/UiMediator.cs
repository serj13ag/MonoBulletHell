using MonoBulletHell.Services;

namespace MonoBulletHell.Ui;

public interface IUiMediator
{
    void ResolutionScaleSelected(float scale);
}

public class UiMediator : IUiMediator
{
    private readonly IScreenService _screenService;

    public UiMediator(IScreenService screenService)
    {
        _screenService = screenService;
    }

    public void ResolutionScaleSelected(float scale)
    {
        _screenService.ApplyScale(scale);
    }
}