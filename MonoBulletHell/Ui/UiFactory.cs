using MonoBulletHell.Ui.Elements;
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
        return new TitlePanel(this);
    }

    public OptionsPanel CreateOptionsPanel()
    {
        return new OptionsPanel(this, _uiMediator);
    }

    public PausePanel CreatePausePanel()
    {
        return new PausePanel(this);
    }

    public GameOverPanel CreateGameOverPanel()
    {
        return new GameOverPanel(this);
    }

    public CustomButton CreateCustomButton()
    {
        return new CustomButton(_uiMediator);
    }
}