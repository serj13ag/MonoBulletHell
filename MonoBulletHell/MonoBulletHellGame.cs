using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoBulletHell.Scenes;
using MonoBulletHell.Services;

namespace MonoBulletHell;

public class MonoBulletHellGame : Game
{
    private readonly CompositionRoot _root;

    private IInputService _inputService;
    private ISceneService _sceneService;

    public MonoBulletHellGame()
    {
        _root = new CompositionRoot();

        var graphics = new GraphicsDeviceManager(this);
        graphics.PreferredBackBufferWidth = Constants.VirtualWidth;
        graphics.PreferredBackBufferHeight = Constants.VirtualHeight;
        graphics.IsFullScreen = false;

        Content.RootDirectory = "Content";

        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();

        var spriteBatch = new SpriteBatch(GraphicsDevice);
        _root.Initialize(Content, GraphicsDevice, spriteBatch);

        _inputService = _root.GetInstance<IInputService>();
        _sceneService = _root.GetInstance<ISceneService>();

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