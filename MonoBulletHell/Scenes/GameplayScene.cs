using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Core.Scenes;
using MonoBulletHell.Gameplay.Entities;
using MonoBulletHell.Gameplay.Factories;
using MonoBulletHell.Gameplay.Services;
using MonoBulletHell.Services;

namespace MonoBulletHell.Scenes;

public class GameplayScene : BaseScene
{
    private readonly IInputService _inputService;
    private readonly IContentService _contentService;
    private readonly ITimeService _timeService;
    private readonly IBulletService _bulletService;
    private readonly IGameFactory _gameFactory;

    private Ship _ship;
    private Enemy _enemy;

    public GameplayScene(ContentManager content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch,
        IInputService inputService, ITimeService timeService, IContentService contentService, IBulletService bulletService,
        IGameFactory gameFactory)
        : base(content, graphicsDevice, spriteBatch)
    {
        _inputService = inputService;
        _contentService = contentService;
        _timeService = timeService;
        _bulletService = bulletService;
        _gameFactory = gameFactory;
    }

    public override void Initialize()
    {
        base.Initialize();

        _inputService.SetExitOnEscapeKeyPressed(false);
    }

    public override void LoadContent()
    {
        base.LoadContent();

        _contentService.Load(Content);
    }

    public override void Enter()
    {
        base.Enter();

        _ship = _gameFactory.CreateShip(new Vector2(Constants.VirtualWidth / 2f, 900f));
        _enemy = _gameFactory.CreateEnemy(new Vector2(Constants.VirtualWidth / 2f, 100f));
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _timeService.Update(gameTime);
        _bulletService.Update();

        _ship.Update();
        _enemy.Update();
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _ship.Draw(SpriteBatch);
        _enemy.Draw(SpriteBatch);

        _bulletService.Draw(SpriteBatch);

        SpriteBatch.End();
    }
}