using Microsoft.Xna.Framework;
using MonoBulletHell.Helpers;
using MonoBulletHell.Services;
using MonoBulletHell.Ui.Elements;
using MonoBulletHell.Ui.Elements.Panels;

namespace MonoBulletHell.Ui;

public interface IUiFactory
{
    TitlePanel CreateTitlePanel();
    OptionsPanel CreateOptionsPanel();
    PausePanel CreatePausePanel();
    GameOverPanel CreateGameOverPanel();
    CustomButton CreateCustomButton();
}

public class UiFactory : IUiFactory
{
    private readonly IContentService _contentService;
    private readonly IUiMediator _uiMediator;

    private Color PanelColor => ColorHelper.FromHex(_contentService.GetColorConfig().UiPanel);
    private Color ButtonColor => ColorHelper.FromHex(_contentService.GetColorConfig().UiButton);

    public UiFactory(IContentService contentService, IUiMediator uiMediator)
    {
        _contentService = contentService;
        _uiMediator = uiMediator;
    }

    public TitlePanel CreateTitlePanel()
    {
        return new TitlePanel(this);
    }

    public OptionsPanel CreateOptionsPanel()
    {
        return new OptionsPanel(this, _uiMediator, PanelColor);
    }

    public PausePanel CreatePausePanel()
    {
        return new PausePanel(this, PanelColor);
    }

    public GameOverPanel CreateGameOverPanel()
    {
        return new GameOverPanel(this, PanelColor);
    }

    public CustomButton CreateCustomButton()
    {
        return new CustomButton(_uiMediator, ButtonColor);
    }
}