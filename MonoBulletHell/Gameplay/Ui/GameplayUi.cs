using System;
using Gum.Forms.Controls;
using Microsoft.Xna.Framework;
using MonoBulletHell.Ui;
using MonoGameGum;
using MonoGameGum.GueDeriving;

namespace MonoBulletHell.Gameplay.Ui;

public class GameplayUi : ContainerRuntime
{
    private readonly GumService _gumService;

    private readonly Panel _pausePanel;
    private readonly Button _resumeButton;

    private readonly Panel _gameOverPanel;
    private readonly Button _restartButton;

    public event Action ResumeButtonClicked;
    public event Action RestartButtonClicked;
    public event Action QuitButtonClicked;

    public GameplayUi(GumService gumService)
    {
        _gumService = gumService;

        Dock(Gum.Wireframe.Dock.Fill);
        this.AddToRoot();

        _pausePanel = CreatePausePanel(out _resumeButton);
        AddChild(_pausePanel.Visual);
        _pausePanel.IsVisible = false;

        _gameOverPanel = CreateGameOverPanel(out _restartButton);
        AddChild(_gameOverPanel.Visual);
        _gameOverPanel.IsVisible = false;
    }

    public void Update(GameTime gameTime)
    {
        _gumService.Update(gameTime);
    }

    public void Draw()
    {
        _gumService.Draw();
    }

    public void ShowPausePanel()
    {
        _pausePanel.IsVisible = true;
        _resumeButton.IsFocused = true;
    }

    public void HidePausePanel()
    {
        _pausePanel.IsVisible = false;
        _resumeButton.IsFocused = false; // TODO: reset focus from all?
    }

    public void ShowGameOverPanel()
    {
        _gameOverPanel.IsVisible = true;
        _restartButton.IsFocused = true;
    }

    private void HideGameOverPanel()
    {
        _gameOverPanel.IsVisible = false;
        _restartButton.IsFocused = false;
    }

    private void OnResumeButtonClicked(object sender, EventArgs e)
    {
        HidePausePanel();
        ResumeButtonClicked?.Invoke();
    }

    private void OnRestartButtonClicked(object sender, EventArgs e)
    {
        HidePausePanel();
        HideGameOverPanel();
        RestartButtonClicked?.Invoke();
    }

    private void OnQuitButtonClicked(object sender, EventArgs e)
    {
        HidePausePanel();
        QuitButtonClicked?.Invoke();
    }

    private Panel CreatePausePanel(out Button resumeButton)
    {
        var panel = new Panel();
        panel.Anchor(Gum.Wireframe.Anchor.Center);
        panel.Width = 100f;
        panel.Height = 50f;

        var background = new ColoredRectangleRuntime();
        panel.AddChild(background);
        background.Dock(Gum.Wireframe.Dock.Fill);
        background.Color = Color.DarkSlateBlue;

        var titleText = new Label();
        panel.AddChild(titleText);
        titleText.Anchor(Gum.Wireframe.Anchor.Top);
        titleText.Text = UiConstants.PausePanelTitleText;
        titleText.Y = 10f;

        var buttonsPanel = new StackPanel();
        panel.AddChild(buttonsPanel);
        buttonsPanel.Anchor(Gum.Wireframe.Anchor.Bottom);
        buttonsPanel.Y = -10f;
        buttonsPanel.Spacing = 5f;

        resumeButton = new Button();
        buttonsPanel.AddChild(resumeButton);
        resumeButton.Text = UiConstants.ResumeButtonText;
        resumeButton.Click += OnResumeButtonClicked;

        var restartButton = new Button();
        buttonsPanel.AddChild(restartButton);
        restartButton.Text = UiConstants.RestartButtonText;
        restartButton.Click += OnRestartButtonClicked;

        var quitButton = new Button();
        buttonsPanel.AddChild(quitButton);
        quitButton.Text = UiConstants.QuitButtonText;
        quitButton.Click += OnQuitButtonClicked;

        return panel;
    }

    private Panel CreateGameOverPanel(out Button restartButton)
    {
        var panel = new Panel();
        panel.Anchor(Gum.Wireframe.Anchor.Center);
        panel.Width = 100f;
        panel.Height = 50f;

        var background = new ColoredRectangleRuntime();
        panel.AddChild(background);
        background.Dock(Gum.Wireframe.Dock.Fill);
        background.Color = Color.DarkSlateBlue;

        var titleText = new Label();
        panel.AddChild(titleText);
        titleText.Anchor(Gum.Wireframe.Anchor.Top);
        titleText.Text = UiConstants.GameOverPanelTitleText;
        titleText.Y = 10f;

        var buttonsPanel = new StackPanel();
        panel.AddChild(buttonsPanel);
        buttonsPanel.Anchor(Gum.Wireframe.Anchor.Bottom);
        buttonsPanel.Y = -10f;
        buttonsPanel.Spacing = 5f;

        restartButton = new Button();
        buttonsPanel.AddChild(restartButton);
        restartButton.Text = UiConstants.RestartButtonText;
        restartButton.Click += OnRestartButtonClicked;

        var quitButton = new Button();
        buttonsPanel.AddChild(quitButton);
        quitButton.Text = UiConstants.QuitButtonText;
        quitButton.Click += OnQuitButtonClicked;

        return panel;
    }
}