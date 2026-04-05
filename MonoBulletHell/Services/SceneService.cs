using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Core.Scenes;
using MonoBulletHell.Scenes;

namespace MonoBulletHell.Services;

public interface ISceneService
{
    void ChangeScene(SceneType sceneType);
    void Update(GameTime gameTime);
    void Draw(GameTime gameTime);
}

public class SceneService : ISceneService
{
    private readonly ContentManager _content;
    private readonly GraphicsDevice _graphicsDevice;
    private readonly SpriteBatch _spriteBatch;
    private readonly IInputService _inputService;

    private BaseScene _nextScene;
    private BaseScene _activeScene;

    public SceneService(ContentManager content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch,
        IInputService inputService)
    {
        _content = content;
        _graphicsDevice = graphicsDevice;
        _spriteBatch = spriteBatch;
        _inputService = inputService;
    }

    public void ChangeScene(SceneType sceneType)
    {
        var nextScene = CreateScene(sceneType);
        if (_activeScene != nextScene)
        {
            _nextScene = nextScene;
        }
    }

    public void Update(GameTime gameTime)
    {
        if (_nextScene != null)
        {
            TransitionScene();
        }

        _activeScene?.Update(gameTime);
    }

    public void Draw(GameTime gameTime)
    {
        _activeScene?.Draw(gameTime);
    }

    private BaseScene CreateScene(SceneType sceneType)
    {
        BaseScene scene = sceneType switch
        {
            SceneType.Gameplay => new GameplayScene(_content, _graphicsDevice, _spriteBatch, _inputService),
            _ => throw new ArgumentOutOfRangeException(nameof(sceneType), sceneType, null),
        };
        return scene;
    }

    private void TransitionScene()
    {
        _activeScene?.UnloadContent();
        _activeScene?.Exit();

        GC.Collect();

        _activeScene = _nextScene;
        _nextScene = null;

        _activeScene?.Initialize();
        _activeScene?.LoadContent();
        _activeScene?.Enter();
    }
}