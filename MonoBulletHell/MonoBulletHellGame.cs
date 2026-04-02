using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoBulletHell.Core.Graphics;
using MonoBulletHell.GameObjects;
using MonoBulletHell.Services;

namespace MonoBulletHell;

public class MonoBulletHellGame : Game
{
    private readonly ContentManager _content;

    private readonly InputService _inputService;
    private readonly TimeService _timeService;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Ship _ship;

    public MonoBulletHellGame()
    {
        _content = Content;
        _content.RootDirectory = "Content";

        _inputService = new InputService();
        _timeService = new TimeService();

        _graphics = new GraphicsDeviceManager(this);

        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        var atlas = TextureAtlas.FromFile(_content, "images/atlas-definition.json");

        var shipSprite = atlas.CreateSprite("ship");
        shipSprite.CenterOrigin();
        _ship = new Ship(_inputService, _timeService, shipSprite);
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

        _spriteBatch.Begin();
        _ship.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}