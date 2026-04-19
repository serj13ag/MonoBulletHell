using MonoBulletHell.Ui.Elements.Panels;

namespace MonoBulletHell.Ui;

public interface IUiFactory
{
    TitlePanel CreateTitlePanel();
    OptionsPanel CreateOptionsPanel();
    PausePanel CreatePausePanel();
    GameOverPanel CreateGameOverPanel();
}

public class UiFactory : IUiFactory
{
    private readonly IUiMediator _uiMediator;

    public UiFactory(IUiMediator uiMediator)
    {
        _uiMediator = uiMediator;
    }

    public TitlePanel CreateTitlePanel()
    {
        return new TitlePanel();
    }

    public OptionsPanel CreateOptionsPanel()
    {
        return new OptionsPanel(_uiMediator);
    }

    public PausePanel CreatePausePanel()
    {
        return new PausePanel();
    }

    public GameOverPanel CreateGameOverPanel()
    {
        return new GameOverPanel();
    }
}