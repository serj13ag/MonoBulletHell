using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Core.Scenes;
using MonoBulletHell.Gameplay.Entities;
using MonoBulletHell.Gameplay.Factories;
using MonoBulletHell.Gameplay.Services;
using MonoBulletHell.Gameplay.Ui;
using MonoBulletHell.Helpers;
using MonoBulletHell.Services;
using MonoGameGum;

namespace MonoBulletHell.Scenes;

public class GameplayScene : BaseScene
{
    private enum GameState
    {
        Playing,
        Paused,
    }

    private readonly GumService _gumService;
    private readonly IInputService _inputService;
    private readonly ISceneService _sceneService;
    private readonly IContentService _contentService;
    private readonly ITimeService _timeService;
    private readonly IBulletService _bulletService;
    private readonly IGameFactory _gameFactory;
    private readonly IEnemyService _enemyService;

    private GameState _gameState;

    private GameplayUi _ui;

    private Ship _ship;

    public GameplayScene(ContentManager content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, GumService gumService,
        IInputService inputService, ISceneService sceneService, ITimeService timeService, IContentService contentService,
        IBulletService bulletService, IGameFactory gameFactory, IEnemyService enemyService)
        : base(content, graphicsDevice, spriteBatch)
    {
        _gumService = gumService;
        _inputService = inputService;
        _sceneService = sceneService;
        _contentService = contentService;
        _timeService = timeService;
        _bulletService = bulletService;
        _gameFactory = gameFactory;
        _enemyService = enemyService;
    }

    public override void Initialize()
    {
        base.Initialize();

        _inputService.SetExitOnEscapeKeyPressed(false);

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

        _gameState = GameState.Playing;

        _ship = _gameFactory.CreateShip(ScreenHelper.GetLerpScreenVirtualPosition(0.5f, 0.8f));
        _ship.OnDestroyed += OnShipDestroyed;

        _enemyService.SpawnEnemy(ScreenHelper.GetLerpScreenVirtualPosition(0.2f, 0.2f));
        _enemyService.SpawnEnemy(ScreenHelper.GetLerpScreenVirtualPosition(0.8f, 0.2f));
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _ui.Update(gameTime);

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
        _enemyService.Update();

        _ship.Update();
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _ship.Draw(SpriteBatch);

        _bulletService.Draw(SpriteBatch);
        _enemyService.Draw(SpriteBatch);

        SpriteBatch.End();

        _ui.Draw();
    }

    private void OnShipDestroyed(object sender, EventArgs e)
    {
        _sceneService.ChangeScene(SceneType.Title); // TODO: show end screen
    }

    private void OnResumeButtonClicked()
    {
        _gameState = GameState.Playing;
    }

    private void OnRetryButtonClicked()
    {
        // TODO: impl
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

        _ui = new GameplayUi(_gumService);

        _ui.ResumeButtonClicked += OnResumeButtonClicked;
        _ui.RetryButtonClicked += OnRetryButtonClicked;
        _ui.QuitButtonClicked += OnQuitButtonClicked;
    }
}