using System;
using Gum.Forms.Controls;
using MonoGameGum.GueDeriving;

namespace MonoBulletHell.Ui.Elements.Panels;

public class GameOverPanel : BasePanel
{
    private readonly Button _restartButton;

    protected override Button FocusButton => _restartButton;

    public event Action OnRestartButtonClicked;
    public event Action OnQuitButtonClicked;

    public GameOverPanel()
    {
        Anchor(Gum.Wireframe.Anchor.Center);
        Width = 100f;
        Height = 50f;

        var background = new ColoredRectangleRuntime();
        AddChild(background);
        background.Dock(Gum.Wireframe.Dock.Fill);
        background.Color = Constants.Colors.BackgroundDark;

        var titleText = new CustomLabel();
        AddChild(titleText);
        titleText.Anchor(Gum.Wireframe.Anchor.Top);
        titleText.Text = UiConstants.GameOverPanelTitleText;
        titleText.Y = 10f;

        var buttonsPanel = new StackPanel();
        AddChild(buttonsPanel);
        buttonsPanel.Anchor(Gum.Wireframe.Anchor.Bottom);
        buttonsPanel.Y = -10f;
        buttonsPanel.Spacing = 5f;

        _restartButton = new CustomButton();
        buttonsPanel.AddChild(_restartButton);
        _restartButton.Text = UiConstants.RestartButtonText;
        _restartButton.Click += RestartButtonClicked;

        var quitButton = new CustomButton();
        buttonsPanel.AddChild(quitButton);
        quitButton.Text = UiConstants.QuitButtonText;
        quitButton.Click += QuitButtonClicked;
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