using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Core.Scenes;
using MonoBulletHell.Gameplay.Entities;
using MonoBulletHell.Gameplay.Factories;
using MonoBulletHell.Gameplay.Services;
using MonoBulletHell.Helpers;
using MonoBulletHell.Services;
using MonoBulletHell.Ui;
using MonoGameGum;

namespace MonoBulletHell.Scenes;

public class GameplayScene : BaseScene
{
    private enum GameState
    {
        Playing,
        Paused,
        GameOver,
    }

    private readonly GumService _gumService;
    private readonly IUiFactory _uiFactory;
    private readonly IInputService _inputService;
    private readonly ISceneService _sceneService;
    private readonly IContentService _contentService;
    private readonly ITimeService _timeService;
    private readonly IBulletService _bulletService;
    private readonly IGameFactory _gameFactory;
    private readonly IEnemyService _enemyService;
    private readonly IEnemySpawnService _enemySpawnService;
    private readonly IBackgroundService _backgroundService;
    private readonly IParticleService _particleService;
    private readonly IRenderService _renderService;

    private GameState _gameState;

    private GameplayUi _ui;

    private Ship _ship;

    public GameplayScene(ContentManager content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, GumService gumService,
        IUiFactory uiFactory, IInputService inputService, ISceneService sceneService, ITimeService timeService,
        IContentService contentService, IBulletService bulletService, IGameFactory gameFactory, IEnemyService enemyService,
        IEnemySpawnService enemySpawnService, IBackgroundService backgroundService, IParticleService particleService,
        IRenderService renderService)
        : base(content, graphicsDevice, spriteBatch)
    {
        _gumService = gumService;
        _uiFactory = uiFactory;
        _inputService = inputService;
        _sceneService = sceneService;
        _contentService = contentService;
        _timeService = timeService;
        _bulletService = bulletService;
        _gameFactory = gameFactory;
        _enemyService = enemyService;
        _enemySpawnService = enemySpawnService;
        _backgroundService = backgroundService;
        _particleService = particleService;
        _renderService = renderService;
    }

    public override void Initialize()
    {
        base.Initialize();

        InitializeUi();
    }

    public override void LoadContent()
    {
        base.LoadContent();

        _contentService.Load(Content);
    }

    public override void Enter()
    {
        base.Enter();

        _ship = _gameFactory.CreateShip();
        _ship.OnDestroyed += OnShipDestroyed;

        InitializeNewGame();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _ui.Update(gameTime);

        if (_gameState == GameState.GameOver)
        {
            return;
        }

        if (_inputService.PausePressed())
        {
            TogglePause();
        }

        if (_gameState == GameState.Paused)
        {
            return;
        }

        _timeService.Update(gameTime);
        _bulletService.Update();
        _enemySpawnService.Update();
        _enemyService.Update();
        _backgroundService.Update();
        _particleService.Update();

        _ship.Update();
    }

    public override void LateUpdate(GameTime gameTime)
    {
        base.LateUpdate(gameTime);

        _backgroundService.Render(_renderService);
        _ship.Render(_renderService);

        _bulletService.Render(_renderService);
        _enemyService.Render(_renderService);
        _particleService.Render(_renderService);
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        GraphicsDevice.Clear(Constants.Colors.BackgroundColor);

        _renderService.Draw(SpriteBatch);

        _ui.Draw();
    }

    private void InitializeNewGame()
    {
        _bulletService.Clear();
        _enemyService.Clear();
        _particleService.Clear();

        _backgroundService.Initialize(_contentService.BackgroundTexture);

        _enemySpawnService.Initialize(_contentService.GetSpawnConfig());

        _ship.InitializeAt(ScreenHelper.GetLerpScreenVirtualPosition(0.5f, 0.8f));

        _gameState = GameState.Playing;
    }

    private void OnShipDestroyed(object sender, EventArgs e)
    {
        _gameState = GameState.GameOver;
        _ui.ShowGameOverPanel();
    }

    private void OnResumeButtonClicked()
    {
        _gameState = GameState.Playing;
    }

    private void OnRestartButtonClicked()
    {
        InitializeNewGame();
    }

    private void OnQuitButtonClicked()
    {
        _sceneService.ChangeScene(SceneType.Title);
    }

    private void TogglePause()
    {
        if (_gameState == GameState.Paused)
        {
            _ui.HidePausePanel();
            _gameState = GameState.Playing;
        }
        else
        {
            _ui.ShowPausePanel();
            _gameState = GameState.Paused;
        }
    }

    private void InitializeUi()
    {
        _gumService.Root.Children.Clear();

        _ui = new GameplayUi(_gumService, _uiFactory);

        _ui.ResumeButtonClicked += OnResumeButtonClicked;
        _ui.RestartButtonClicked += OnRestartButtonClicked;
        _ui.QuitButtonClicked += OnQuitButtonClicked;
    }
}