using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoBulletHell.Core.Graphics;
using MonoBulletHell.GameObjects;

namespace MonoBulletHell;

public class MonoBulletHellGame : Game
{
    private readonly ContentManager _content;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Ship _ship;

    public MonoBulletHellGame()
    {
        _content = Content;
        _content.RootDirectory = "Content";

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
        _ship = new Ship(shipSprite);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

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