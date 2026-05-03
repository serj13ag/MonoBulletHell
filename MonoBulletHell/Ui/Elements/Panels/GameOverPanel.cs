using System;
using Gum.Forms.Controls;
using MonoGameGum.GueDeriving;

namespace MonoBulletHell.Ui.Elements.Panels;

public class GameOverPanel : BasePanel
{
    private readonly CustomLabel _titleText;
    private readonly Button _restartButton;

    protected override Button FocusButton => _restartButton;

    public event Action OnRestartButtonClicked;
    public event Action OnQuitButtonClicked;

    public GameOverPanel(UiFactory uiFactory)
    {
        Anchor(Gum.Wireframe.Anchor.Center);
        Width = 100f;
        Height = 50f;

        var background = new ColoredRectangleRuntime();
        AddChild(background);
        background.Dock(Gum.Wireframe.Dock.Fill);
        background.Color = Constants.Colors.BackgroundDark;

        _titleText = new CustomLabel();
        AddChild(_titleText);
        _titleText.Anchor(Gum.Wireframe.Anchor.Top);
        _titleText.Y = 10f;

        var buttonsPanel = new StackPanel();
        AddChild(buttonsPanel);
        buttonsPanel.Anchor(Gum.Wireframe.Anchor.Bottom);
        buttonsPanel.Y = -10f;
        buttonsPanel.Spacing = 5f;

        _restartButton = uiFactory.CreateCustomButton();
        buttonsPanel.AddChild(_restartButton);
        _restartButton.Text = UiConstants.RestartButtonText;
        _restartButton.Click += RestartButtonClicked;

        var quitButton = uiFactory.CreateCustomButton();
        buttonsPanel.AddChild(quitButton);
        quitButton.Text = UiConstants.QuitButtonText;
        quitButton.Click += QuitButtonClicked;
    }

    public void UpdateTitle(bool isWin)
    {
        _titleText.Text = isWin ? UiConstants.GameOverPanelWinTitleText : UiConstants.GameOverPanelLostTitleText;
    }

    private void RestartButtonClicked(object sender, EventArgs e)
    {
        OnRestartButtonClicked?.Invoke();
    }

    private void QuitButtonClicked(object sender, EventArgs e)
    {
        OnQuitButtonClicked?.Invoke();
    }
}