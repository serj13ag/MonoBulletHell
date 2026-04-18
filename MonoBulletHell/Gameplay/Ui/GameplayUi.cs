using System;
using Microsoft.Xna.Framework;
using MonoBulletHell.Ui.Panels;
using MonoGameGum;
using MonoGameGum.GueDeriving;

namespace MonoBulletHell.Gameplay.Ui;

public class GameplayUi : ContainerRuntime
{
    private readonly GumService _gumService;

    private readonly PausePanel _pausePanel;
    private readonly OptionsPanel _optionsPanel;
    private readonly GameOverPanel _gameOverPanel;

    public event Action ResumeButtonClicked;
    public event Action RestartButtonClicked;
    public event Action QuitButtonClicked;

    public GameplayUi(GumService gumService)
    {
        _gumService = gumService;

        Dock(Gum.Wireframe.Dock.Fill);
        this.AddToRoot();

        _pausePanel = new PausePanel();
        AddChild(_pausePanel.Visual);
        _pausePanel.OnResumeButtonClicked += OnResumeButtonClicked;
        _pausePanel.OnRestartButtonClicked += OnRestartButtonClicked;
        _pausePanel.OnOptionsButtonClicked += OnOptionsButtonClicked;
        _pausePanel.OnQuitButtonClicked += OnQuitButtonClicked;
        _pausePanel.Disable();

        _optionsPanel = new OptionsPanel();
        _optionsPanel.AddToRoot();
        _optionsPanel.OnBackButtonClicked += OnOptionsBackButtonClicked;
        _optionsPanel.Disable();

        _gameOverPanel = new GameOverPanel();
        AddChild(_gameOverPanel.Visual);
        _gameOverPanel.Disable();
        _gameOverPanel.OnRestartButtonClicked += OnRestartButtonClicked;
        _gameOverPanel.OnQuitButtonClicked += OnQuitButtonClicked;
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
        _pausePanel.Enable();
    }

    public void HidePausePanel()
    {
        _pausePanel.Disable();
    }

    public void ShowGameOverPanel()
    {
        _gameOverPanel.Enable();
    }

    private void HideGameOverPanel()
    {
        _gameOverPanel.Disable();
    }

    private void OnResumeButtonClicked()
    {
        HidePausePanel();
        ResumeButtonClicked?.Invoke();
    }

    private void OnRestartButtonClicked()
    {
        HidePausePanel();
        HideGameOverPanel();
        RestartButtonClicked?.Invoke();
    }

    private void OnQuitButtonClicked()
    {
        HidePausePanel();
        QuitButtonClicked?.Invoke();
    }

    private void OnOptionsButtonClicked()
    {
        _optionsPanel.Enable();
    }

    private void OnOptionsBackButtonClicked()
    {
        _optionsPanel.Disable();
    }
}