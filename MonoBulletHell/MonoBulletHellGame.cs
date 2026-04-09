using Gum.Forms.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoBulletHell.Scenes;
using MonoBulletHell.Services;
using MonoGameGum;

namespace MonoBulletHell;

public class MonoBulletHellGame : Game
{
    private readonly CompositionRoot _root;

    private IInputService _inputService;
    private ISceneService _sceneService;
    private IDebugService _debugService;

    public MonoBulletHellGame()
    {
        _root = new CompositionRoot(this);

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
        _debugService = _root.GetInstance<IDebugService>();

        InitializeGum(Content);

        _sceneService.ChangeScene(SceneType.Title);
    }

    protected override void Update(GameTime gameTime)
    {
        _inputService.Update();
        _debugService.Update();

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
        _debugService.Render();

        base.Draw(gameTime);
    }

    private void InitializeGum(ContentManager content)
    {
        var gumService = _root.GetInstance<GumService>();

        gumService.Initialize(this);
        gumService.ContentLoader.XnaContentManager = content;

        FrameworkElement.KeyboardsForUiControl.Add(gumService.Keyboard);
        FrameworkElement.GamePadsForUiControl.AddRange(gumService.Gamepads);
        FrameworkElement.TabReverseKeyCombos.Add(new KeyCombo() { PushedKey = Keys.Up });
        FrameworkElement.TabKeyCombos.Add(new KeyCombo() { PushedKey = Keys.Down });

        gumService.CanvasWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
        gumService.CanvasHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;
    }
}