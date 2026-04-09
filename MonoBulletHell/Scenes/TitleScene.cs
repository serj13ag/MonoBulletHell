using System;
using Gum.Forms.Controls;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Core.Scenes;
using MonoBulletHell.Services;
using MonoGameGum;

namespace MonoBulletHell.Scenes;

public class TitleScene : BaseScene
{
    private const string TitleText = "MONO HELL";
    private const string StartButtonText = "START GAME";
    private const string QuitButtonText = "QUIT";

    private readonly Game _game;
    private readonly GumService _gumService;
    private readonly ISceneService _sceneService;

    public TitleScene(Game game, ContentManager contentManager, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch,
        GumService gumService, ISceneService sceneService)
        : base(contentManager, graphicsDevice, spriteBatch)
    {
        _game = game;
        _gumService = gumService;
        _sceneService = sceneService;
    }

    public override void Initialize()
    {
        base.Initialize();

        // Core.ExitOnEscape = true; TODO: impl

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

        GraphicsDevice.Clear(new Color(32, 40, 78, 255));

        _gumService.Draw();
    }

    private void InitializeUi()
    {
        _gumService.Root.Children.Clear();

        var mainPanel = new Panel();
        mainPanel.AddToRoot();
        mainPanel.Dock(Dock.Fill);

        var titleText = new Label();
        mainPanel.AddChild(titleText);
        titleText.Anchor(Anchor.Top);
        titleText.Text = TitleText;
        titleText.Y = 100f;

        CreateButtons(mainPanel);
    }

    private void CreateButtons(Panel parentPanel)
    {
        var buttonPanel = new StackPanel();
        parentPanel.AddChild(buttonPanel);
        buttonPanel.Anchor(Anchor.Center);
        buttonPanel.Spacing = 5f;

        var startButton = new Button();
        buttonPanel.AddChild(startButton);
        startButton.Text = StartButtonText;
        startButton.Width = 200;
        startButton.IsFocused = true;
        startButton.Click += OnStartButtonClicked;

        var quitButton = new Button();
        buttonPanel.AddChild(quitButton);
        quitButton.Text = QuitButtonText;
        quitButton.Width = 200;
        quitButton.Click += OnQuitButtonClicked;
    }

    private void OnStartButtonClicked(object o, EventArgs eventArgs)
    {
        _sceneService.ChangeScene(SceneType.Gameplay);
    }

    private void OnQuitButtonClicked(object o, EventArgs eventArgs)
    {
        _game.Exit();
    }
}