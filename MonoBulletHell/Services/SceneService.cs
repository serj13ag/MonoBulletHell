using System;
using LightInject;
using Microsoft.Xna.Framework;
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
    private readonly Func<Scope> _scopeFactory;

    private Scope _sceneScope;

    private SceneType? _nextSceneType;
    private SceneType? _activeSceneType;
    private BaseScene _activeScene;

    public SceneService(Func<Scope> scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public void ChangeScene(SceneType sceneType)
    {
        if (_activeSceneType != sceneType)
        {
            _nextSceneType = sceneType;
        }
    }

    public void Update(GameTime gameTime)
    {
        if (_nextSceneType != null)
        {
            TransitionScene();
        }

        _activeScene?.Update(gameTime);
    }

    public void Draw(GameTime gameTime)
    {
        _activeScene?.Draw(gameTime);
    }

    private void TransitionScene()
    {
        _activeScene?.UnloadContent();
        _activeScene?.Exit();

        _sceneScope?.Dispose();

        GC.Collect();

        _sceneScope = _scopeFactory.Invoke();
        BaseScene nextScene = _nextSceneType switch
        {
            SceneType.Gameplay => _sceneScope.GetInstance<GameplayScene>(),
            _ => throw new ArgumentOutOfRangeException(),
        };

        _activeScene = nextScene;
        _activeSceneType = _nextSceneType;
        _nextSceneType = null;

        _activeScene?.Initialize();
        _activeScene?.LoadContent();
        _activeScene?.Enter();
    }
}