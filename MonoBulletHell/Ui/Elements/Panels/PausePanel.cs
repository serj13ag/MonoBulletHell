using System;
using Gum.Forms.Controls;
using Microsoft.Xna.Framework;
using MonoGameGum.GueDeriving;

namespace MonoBulletHell.Ui.Elements.Panels;

public class PausePanel : BasePanel
{
    private readonly Button _resumeButton;

    protected override Button FocusButton => _resumeButton;

    public event Action OnResumeButtonClicked;
    public event Action OnRestartButtonClicked;
    public event Action OnOptionsButtonClicked;
    public event Action OnQuitButtonClicked;

    public PausePanel()
    {
        Anchor(Gum.Wireframe.Anchor.Center);
        Width = 100f;
        Height = 50f;

        var background = new ColoredRectangleRuntime();
        AddChild(background);
        background.Dock(Gum.Wireframe.Dock.Fill);
        background.Color = Color.DarkSlateBlue;

        var titleText = new CustomLabel();
        AddChild(titleText);
        titleText.Anchor(Gum.Wireframe.Anchor.Top);
        titleText.Text = UiConstants.PausePanelTitleText;
        titleText.Y = 10f;

        var buttonsPanel = new StackPanel();
        AddChild(buttonsPanel);
        buttonsPanel.Anchor(Gum.Wireframe.Anchor.Bottom);
        buttonsPanel.Y = -10f;
        buttonsPanel.Spacing = 5f;

        _resumeButton = new CustomButton();
        buttonsPanel.AddChild(_resumeButton);
        _resumeButton.Text = UiConstants.ResumeButtonText;
        _resumeButton.Click += ResumeButtonClicked;

        var restartButton = new CustomButton();
        buttonsPanel.AddChild(restartButton);
        restartButton.Text = UiConstants.RestartButtonText;
        restartButton.Click += RestartButtonClicked;

        var optionsButton = new CustomButton();
        buttonsPanel.AddChild(optionsButton);
        optionsButton.Text = UiConstants.OptionsButtonText;
        optionsButton.Click += OptionsButtonClicked;
        
        var quitButton = new CustomButton();
        buttonsPanel.AddChild(quitButton);
        quitButton.Text = UiConstants.QuitButtonText;
        quitButton.Click += QuitButtonClicked;
    }

    private void ResumeButtonClicked(object sender, EventArgs e)
    {
        OnResumeButtonClicked?.Invoke();
    }

    private void RestartButtonClicked(object sender, EventArgs e)
    {
        OnRestartButtonClicked?.Invoke();
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