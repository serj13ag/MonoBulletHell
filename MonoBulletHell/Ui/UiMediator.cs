using MonoBulletHell.Services;

namespace MonoBulletHell.Ui;

public interface IUiMediator
{
    void ResolutionScaleSelected(float scale);
}

public class UiMediator : IUiMediator
{
    private readonly IGameService _gameService;

    public UiMediator(IGameService gameService)
    {
        _gameService = gameService;
    }

    public void ResolutionScaleSelected(float scale)
    {
        _gameService.ApplyScale(scale);
    }
}