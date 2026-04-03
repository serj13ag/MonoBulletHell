using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoBulletHell.GameObjects;
using MonoBulletHell.Services;

namespace MonoBulletHell;

public class MonoBulletHellGame : Game
{
    private readonly ContentManager _content;

    private readonly InputService _inputService;
    private readonly TimeService _timeService;
    private readonly ContentService _contentService;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Ship _ship;

    public MonoBulletHellGame()
    {
        _content = Content;
        _content.RootDirectory = "Content";

        _inputService = new InputService();
        _timeService = new TimeService();
        _contentService = new ContentService();

        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 1920;
        _graphics.PreferredBackBufferHeight = 1080;
        _graphics.IsFullScreen = false;

        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();

        _ship = new Ship(_inputService, _timeService, _contentService);
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _contentService.Load(_content);
    }

    protected override void Update(GameTime gameTime)
    {
        _inputService.Update();
        _timeService.Update(gameTime);

        if (_inputService.Keyboard.WasKeyJustPressed(Keys.Escape))
        {
            Exit();
        }

        _ship.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _ship.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}