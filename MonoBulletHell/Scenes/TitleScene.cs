using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Audio;
using MonoBulletHell.Core.Input;
using MonoBulletHell.Core.Scenes;
using MonoBulletHell.Services;
using MonoBulletHell.Ui;
using MonoBulletHell.Ui.Elements;
using MonoBulletHell.Ui.Elements.Panels;
using MonoGameGum;

namespace MonoBulletHell.Scenes;

public class TitleScene : BaseScene
{
    private readonly IGameService _gameService;
    private readonly GumService _gumService;
    private readonly IUiFactory _uiFactory;
    private readonly IInputService _inputService;
    private readonly ISceneService _sceneService;
    private readonly ISoundService _soundService;
    private readonly IContentService _contentService;

    private OptionsPanel _optionsPanel;

    public TitleScene(IGameService gameService, ContentManager contentManager, GraphicsDevice graphicsDevice,
        SpriteBatch spriteBatch, GumService gumService, IUiFactory uiFactory, IInputService inputService,
        ISceneService sceneService, ISoundService soundService, IContentService contentService)
        : base(contentManager, graphicsDevice, spriteBatch)
    {
        _gameService = gameService;
        _gumService = gumService;
        _uiFactory = uiFactory;
        _inputService = inputService;
        _sceneService = sceneService;
        _soundService = soundService;
        _contentService = contentService;
    }

    public override void Initialize()
    {
        base.Initialize();

        InitializeUi();
    }

    public override void Enter()
    {
        base.Enter();

        _soundService.PlaySong(SongType.Menu);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (_inputService.CancelWasJustPressed())
        {
            if (_optionsPanel.IsVisible)
            {
                _optionsPanel.Disable();
            }
            else
            {
                _gameService.Exit();
            }
        }

        _gumService.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        GraphicsDevice.Clear(_contentService.GetColorConfig().TitleSceneBackground);

        _gumService.Draw();
    }

    public override void Exit()
    {
        base.Exit();

        _soundService.StopAll();
    }

    private void InitializeUi()
    {
        _gumService.Root.Children.Clear();

        var titlePanel = _uiFactory.CreateTitlePanel();
        titlePanel.AddToRoot();
        titlePanel.OnStartButtonClicked += StartButtonClicked;
        titlePanel.OnOptionsButtonClicked += OptionsButtonClicked;
        titlePanel.OnQuitButtonClicked += QuitButtonClicked;

        _optionsPanel = _uiFactory.CreateOptionsPanel();
        _optionsPanel.AddToRoot();
        _optionsPanel.OnBackButtonClicked += OnOptionsBackButtonClicked;
        _optionsPanel.Disable();

        var controlsLabel = new CustomLabel();
        controlsLabel.AddToRoot();
        controlsLabel.Anchor(Anchor.Bottom);
        controlsLabel.Y = -20;
        controlsLabel.Text = "MOVE: WASD\nFOCUS: SHIFT\nFIRE: SPACE";
    }

    private void StartButtonClicked()
    {
        _sceneService.ChangeScene(SceneType.Gameplay);
    }

    private void OptionsButtonClicked()
    {
        _optionsPanel.Enable();
    }

    private void OnOptionsBackButtonClicked()
    {
        _optionsPanel.Disable();
    }

    private void QuitButtonClicked()
    {
        _gameService.Exit();
    }
}