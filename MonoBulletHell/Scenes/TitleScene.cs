using System;
using Gum.Forms.Controls;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Core.Scenes;
using MonoBulletHell.Services;
using MonoBulletHell.Ui;
using MonoBulletHell.Ui.Panels;
using MonoGameGum;

namespace MonoBulletHell.Scenes;

public class TitleScene : BaseScene
{
    private readonly IGameService _gameService;
    private readonly GumService _gumService;
    private readonly IUiFactory _uiFactory;
    private readonly IInputService _inputService;
    private readonly ISceneService _sceneService;

    private OptionsPanel _optionsPanel;

    public TitleScene(IGameService gameService, ContentManager contentManager, GraphicsDevice graphicsDevice,
        SpriteBatch spriteBatch, GumService gumService, IUiFactory uiFactory, IInputService inputService,
        ISceneService sceneService)
        : base(contentManager, graphicsDevice, spriteBatch)
    {
        _gameService = gameService;
        _gumService = gumService;
        _uiFactory = uiFactory;
        _inputService = inputService;
        _sceneService = sceneService;
    }

    public override void Initialize()
    {
        base.Initialize();

        _inputService.SetExitOnEscapeKeyPressed(true);

        InitializeUi();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _gumService.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        GraphicsDevice.Clear(Constants.Colors.BackgroundColor);

        _gumService.Draw();
    }

    private void InitializeUi()
    {
        _gumService.Root.Children.Clear();

        var mainPanel = CreateMainPanel();
        mainPanel.AddToRoot();

        _optionsPanel = _uiFactory.CreateOptionsPanel();
        _optionsPanel.AddToRoot();
        _optionsPanel.OnBackButtonClicked += OnOptionsBackButtonClicked;
        _optionsPanel.Disable();
    }

    private Panel CreateMainPanel()
    {
        var mainPanel = new Panel();
        mainPanel.Dock(Dock.Fill);

        var titleText = new Label();
        mainPanel.AddChild(titleText);
        titleText.Anchor(Anchor.Top);
        titleText.Text = UiConstants.TitleText;
        titleText.Y = 100f;

        CreateButtons(mainPanel);

        return mainPanel;
    }

    private void CreateButtons(Panel parentPanel)
    {
        var buttonPanel = new StackPanel();
        parentPanel.AddChild(buttonPanel);
        buttonPanel.Anchor(Anchor.Center);
        buttonPanel.Spacing = 5f;

        var startButton = new Button();
        buttonPanel.AddChild(startButton);
        startButton.Text = UiConstants.StartButtonText;
        startButton.Width = 200f;
        startButton.IsFocused = true;
        startButton.Click += OnStartButtonClicked;

        var optionsButton = new Button();
        buttonPanel.AddChild(optionsButton);
        optionsButton.Text = UiConstants.OptionsButtonText;
        optionsButton.Width = 200f;
        optionsButton.Click += OnOptionsButtonClicked;

        var quitButton = new Button();
        buttonPanel.AddChild(quitButton);
        quitButton.Text = UiConstants.QuitButtonText;
        quitButton.Width = 200f;
        quitButton.Click += OnQuitButtonClicked;
    }

    private void OnStartButtonClicked(object o, EventArgs eventArgs)
    {
        _sceneService.ChangeScene(SceneType.Gameplay);
    }

    private void OnOptionsButtonClicked(object sender, EventArgs e)
    {
        _optionsPanel.Enable();
    }

    private void OnOptionsBackButtonClicked()
    {
        _optionsPanel.Disable();
    }

    private void OnQuitButtonClicked(object o, EventArgs eventArgs)
    {
        _gameService.Exit();
    }
}