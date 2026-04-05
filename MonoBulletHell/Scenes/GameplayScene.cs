using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Core.Scenes;
using MonoBulletHell.Gameplay.Factories;
using MonoBulletHell.Gameplay.GameObjects;
using MonoBulletHell.Gameplay.Services;
using MonoBulletHell.Services;

namespace MonoBulletHell.Scenes;

public class GameplayScene : BaseScene
{
    private readonly ITimeService _timeService;
    private readonly IContentService _contentService;
    private readonly IGameFactory _gameFactory;
    private readonly IBulletService _bulletService;

    private Ship _ship;

    public GameplayScene(ContentManager content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch,
        IInputService inputService)
        : base(content, graphicsDevice, spriteBatch)
    {
        _timeService = new TimeService();
        _contentService = new ContentService();
        _bulletService = new BulletService(_timeService, _contentService);
        _gameFactory = new GameFactory(inputService, _timeService, _contentService, _bulletService);
    }

    public override void LoadContent()
    {
        base.LoadContent();

        _contentService.Load(Content);
    }

    public override void Enter()
    {
        base.Enter();

        _ship = _gameFactory.CreateShip(new Vector2(32f, 32f));
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _timeService.Update(gameTime);
        _bulletService.Update();

        _ship.Update();
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _ship.Draw(SpriteBatch);

        _bulletService.Draw(SpriteBatch);
        
        SpriteBatch.End();
    }
}