using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.App;
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
    private LevelsPanel _levelsPanel;

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

    public override void Enter(object args)
    {
        base.Enter(args);

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

        var levelsCount = _contentService.GetNumberOfLevels();
        _levelsPanel = _uiFactory.CreateLevelsPanel(levelsCount);
        _levelsPanel.AddToRoot();
        _levelsPanel.OnLevelButtonClicked += OnLevelsSelectLevelButtonClicked;
        _levelsPanel.OnBackButtonClicked += OnLevelsBackButtonClicked;
        _levelsPanel.Disable();

        _optionsPanel = _uiFactory.CreateOptionsPanel();
        _optionsPanel.AddToRoot();
        _optionsPanel.OnBackButtonClicked += OnOptionsBackButtonClicked;
        _optionsPanel.Disable();

        var controlsLabel = new CustomLabel();
        controlsLabel.AddToRoot();
        controlsLabel.Anchor(Anchor.Bottom);
        controlsLabel.Text = "MOVE: WASD\nFOCUS: SHIFT\nFIRE: SPACE";

        var versionLabel = new CustomLabel();
        versionLabel.AddToRoot();
        versionLabel.Anchor(Anchor.TopLeft);
        versionLabel.X = 5;
        versionLabel.Text = BuildInfo.GetVersion(true);
    }

    private void StartButtonClicked()
    {
        _levelsPanel.Enable();
    }

    private void OptionsButtonClicked()
    {
        _optionsPanel.Enable();
    }

    private void OnOptionsBackButtonClicked()
    {
        _optionsPanel.Disable();
    }

    private void OnLevelsSelectLevelButtonClicked(int levelIndex)
    {
        _sceneService.ChangeScene(SceneType.Gameplay, new GameplaySceneArgs(levelIndex));
    }

    private void OnLevelsBackButtonClicked()
    {
        _levelsPanel.Disable();
    }

    private void QuitButtonClicked()
    {
        _gameService.Exit();
    }
}