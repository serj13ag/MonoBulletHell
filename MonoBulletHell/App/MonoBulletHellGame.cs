using System;
using Gum.Forms;
using Gum.Forms.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using MonoBulletHell.Core.Input;
using MonoBulletHell.Scenes;
using MonoBulletHell.Services;
using MonoGameGum;

namespace MonoBulletHell.App;

public class MonoBulletHellGame : Game
{
    private readonly CompositionRoot _root;

    private GumService _gumService;
    private IInputService _inputService;
    private IScreenService _screenService;
    private ISceneService _sceneService;
    private IDebugService _debugService;
    private ISettingsService _settingsService;
    private ISoundService _soundService;

    public MonoBulletHellGame()
    {
        _root = new CompositionRoot(this);

        Content.RootDirectory = "Content";

        IsMouseVisible = true;
        Window.AllowUserResizing = false;

        IsFixedTimeStep = true;
        TargetElapsedTime = TimeSpan.FromSeconds(1.0 / GameConstants.TargetFps);
    }

    protected override void Initialize()
    {
        base.Initialize();

        _root.Initialize(Content, GraphicsDevice);

        _gumService = _root.GetInstance<GumService>();
        _settingsService = _root.GetInstance<ISettingsService>();
        _inputService = _root.GetInstance<IInputService>();
        _screenService = _root.GetInstance<IScreenService>();
        _sceneService = _root.GetInstance<ISceneService>();
        _debugService = _root.GetInstance<IDebugService>();
        _soundService = _root.GetInstance<ISoundService>();

        _settingsService.Initialize();
        _screenService.Initialize();
        _soundService.Initialize();
        _debugService.Initialize(Content);
        InitializeGum(Content);

        _sceneService.ChangeScene(SceneType.Title);
    }

    protected override void LoadContent()
    {
        base.LoadContent();

        var contentService = _root.GetInstance<IContentService>();
        contentService.Load(Content);
    }

    protected override void Update(GameTime gameTime)
    {
        _inputService.Update();
        _debugService.Update(gameTime);
        _soundService.Update();

        _gumService.Cursor.TransformMatrix = _screenService.GetTransformMatrix();

        _sceneService.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        _screenService.SetNativeRenderTarget();

        _sceneService.Draw(gameTime);
        _debugService.Draw();

        _screenService.RenderResultImage();

        base.Draw(gameTime);
    }

    protected override void UnloadContent()
    {
        base.UnloadContent();

        _soundService.Dispose();
    }

    private void InitializeGum(ContentManager content)
    {
        _gumService.Initialize(this, DefaultVisualsVersion.V3);
        _gumService.ContentLoader.XnaContentManager = content;

        FrameworkElement.KeyboardsForUiControl.Add(_gumService.Keyboard);
        FrameworkElement.GamePadsForUiControl.AddRange(_gumService.Gamepads);
        FrameworkElement.TabReverseKeyCombos.Add(new KeyCombo() { PushedKey = Keys.Up });
        FrameworkElement.TabKeyCombos.Add(new KeyCombo() { PushedKey = Keys.Down });

        _gumService.CanvasWidth = GameConstants.VirtualWidth;
        _gumService.CanvasHeight = GameConstants.VirtualHeight;
    }
}