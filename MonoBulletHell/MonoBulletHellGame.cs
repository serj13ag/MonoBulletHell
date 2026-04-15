using System;
using Gum.Forms;
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
    private const double TargetFps = 144.0;

    private readonly CompositionRoot _root;

    private SpriteBatch _spriteBatch;
    private GumService _gumService;
    private IInputService _inputService;
    private ISceneService _sceneService;
    private IDebugService _debugService;

    private RenderTarget2D _nativeRenderTarget;
    private Rectangle _actualScreenRectangle;

    public MonoBulletHellGame()
    {
        _root = new CompositionRoot(this);

        var graphics = new GraphicsDeviceManager(this);
        graphics.PreferredBackBufferWidth = Constants.ActualWidth;
        graphics.PreferredBackBufferHeight = Constants.ActualHeight;
        graphics.IsFullScreen = false;

        Content.RootDirectory = "Content";

        IsMouseVisible = true;

        IsFixedTimeStep = true;
        TargetElapsedTime = TimeSpan.FromSeconds(1.0 / TargetFps);
    }

    protected override void Initialize()
    {
        base.Initialize();

        _root.Initialize(Content, GraphicsDevice);

        _spriteBatch = _root.GetInstance<SpriteBatch>();
        _gumService = _root.GetInstance<GumService>();
        _inputService = _root.GetInstance<IInputService>();
        _sceneService = _root.GetInstance<ISceneService>();
        _debugService = _root.GetInstance<IDebugService>();

        InitializeGum(Content);

        _nativeRenderTarget = new RenderTarget2D(GraphicsDevice, Constants.VirtualWidth, Constants.VirtualHeight);
        _actualScreenRectangle = new Rectangle(0, 0, Constants.ActualWidth, Constants.ActualHeight);

        _sceneService.ChangeScene(SceneType.Title);
    }

    protected override void Update(GameTime gameTime)
    {
        _inputService.Update();
        _debugService.Update();

        UpdateGumCursorTransform();

        _sceneService.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.SetRenderTarget(_nativeRenderTarget);

        _sceneService.Draw(gameTime);
        _debugService.Render();

        // Scale image
        GraphicsDevice.SetRenderTarget(null);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _spriteBatch.Draw(_nativeRenderTarget, _actualScreenRectangle, Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void InitializeGum(ContentManager content)
    {
        _gumService.Initialize(this, DefaultVisualsVersion.V3);
        _gumService.ContentLoader.XnaContentManager = content;

        FrameworkElement.KeyboardsForUiControl.Add(_gumService.Keyboard);
        FrameworkElement.GamePadsForUiControl.AddRange(_gumService.Gamepads);
        FrameworkElement.TabReverseKeyCombos.Add(new KeyCombo() { PushedKey = Keys.Up });
        FrameworkElement.TabKeyCombos.Add(new KeyCombo() { PushedKey = Keys.Down });

        _gumService.CanvasWidth = Constants.VirtualWidth;
        _gumService.CanvasHeight = Constants.VirtualHeight;
    }

    private void UpdateGumCursorTransform()
    {
        var translation = Matrix.CreateTranslation(-_actualScreenRectangle.X, -_actualScreenRectangle.Y, 0);

        var xScale = Constants.VirtualWidth / (float)_actualScreenRectangle.Width;
        var yScale = Constants.VirtualHeight / (float)_actualScreenRectangle.Height;
        var scale = Matrix.CreateScale(xScale, yScale, 1f);

        _gumService.Cursor.TransformMatrix = translation * scale;
    }
}