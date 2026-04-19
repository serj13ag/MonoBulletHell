using System;
using Gum.Forms.Controls;

namespace MonoBulletHell.Ui.Elements.Panels;

public class TitlePanel : Panel
{
    public event Action OnStartButtonClicked;
    public event Action OnOptionsButtonClicked;
    public event Action OnQuitButtonClicked;

    public TitlePanel()
    {
        Dock(Gum.Wireframe.Dock.Fill);

        var titleText = new CustomLabel();
        AddChild(titleText);
        titleText.Anchor(Gum.Wireframe.Anchor.Top);
        titleText.Text = UiConstants.GameTitleText;
        titleText.Y = 100f;

        var buttonsPanel = new StackPanel();
        AddChild(buttonsPanel);
        buttonsPanel.Anchor(Gum.Wireframe.Anchor.Center);
        buttonsPanel.Spacing = 5f;

        var startButton = new CustomButton();
        buttonsPanel.AddChild(startButton);
        startButton.Text = UiConstants.StartButtonText;
        startButton.Width = 200f;
        startButton.IsFocused = true;
        startButton.Click += StartButtonClicked;

        var optionsButton = new CustomButton();
        buttonsPanel.AddChild(optionsButton);
        optionsButton.Text = UiConstants.OptionsButtonText;
        optionsButton.Width = 200f;
        optionsButton.Click += OptionsButtonClicked;

        var quitButton = new CustomButton();
        buttonsPanel.AddChild(quitButton);
        quitButton.Text = UiConstants.QuitButtonText;
        quitButton.Width = 200f;
        quitButton.Click += QuitButtonClicked;
    }

    private void StartButtonClicked(object sender, EventArgs e)
    {
        OnStartButtonClicked?.Invoke();
    }

    private void OptionsButtonClicked(object sender, EventArgs e)
    {
        OnOptionsButtonClicked?.Invoke();
    }

    private void QuitButtonClicked(object sender, EventArgs e)
    {
        OnQuitButtonClicked?.Invoke();
    }
}