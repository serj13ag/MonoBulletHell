using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Core.Scenes;
using MonoBulletHell.GameObjects;
using MonoBulletHell.Gameplay.Factories;
using MonoBulletHell.Services;

namespace MonoBulletHell.Scenes;

public class GameplayScene : BaseScene
{
    private readonly TimeService _timeService;
    private readonly ContentService _contentService;
    private readonly GameFactory _gameFactory;

    private Ship _ship;

    public GameplayScene(ContentManager content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch,
        InputService inputService)
        : base(content, graphicsDevice, spriteBatch)
    {
        _timeService = new TimeService();
        _contentService = new ContentService();
        _gameFactory = new GameFactory(inputService, _timeService, _contentService);
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

        _ship.Update();
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _ship.Draw(SpriteBatch);

        SpriteBatch.End();
    }
}