using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoBulletHell.Scenes;
using MonoBulletHell.Services;

namespace MonoBulletHell;

public class MonoBulletHellGame : Game
{
    private readonly IInputService _inputService;
    private ISceneService _sceneService;

    public MonoBulletHellGame()
    {
        _inputService = new InputService();

        var graphics = new GraphicsDeviceManager(this);
        graphics.PreferredBackBufferWidth = 1920;
        graphics.PreferredBackBufferHeight = 1080;
        graphics.IsFullScreen = false;

        Content.RootDirectory = "Content";

        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();

        var spriteBatch = new SpriteBatch(GraphicsDevice);
        _sceneService = new SceneService(Content, GraphicsDevice, spriteBatch, _inputService);

        _sceneService.ChangeScene(SceneType.Gameplay);
    }

    protected override void Update(GameTime gameTime)
    {
        _inputService.Update();

        if (_inputService.Keyboard.WasKeyJustPressed(Keys.Escape))
        {
            Exit();
        }

        _sceneService.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        _sceneService.Draw(gameTime);

        base.Draw(gameTime);
    }
}