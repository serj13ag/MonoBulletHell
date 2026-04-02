using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoBulletHell.Core.Graphics;

namespace MonoBulletHell;

public class MonoBulletHellGame : Game
{
    private readonly ContentManager _content;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Sprite _shipSprite;

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

        var atlas = TextureAtlas.FromFile(_content, "images/atlas-definition.xml");

        _shipSprite = atlas.CreateSprite("ship");
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
        _shipSprite.Draw(_spriteBatch, Vector2.Zero);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}