using System;
using LightInject;
using Microsoft.Xna.Framework;
using MonoBulletHell.Core.Scenes;

namespace MonoBulletHell.Scenes;

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
        if (_nextSceneType.HasValue)
        {
            TransitionScene(_nextSceneType.Value);
            _nextSceneType = null;
        }

        _activeScene?.Update(gameTime);
    }

    public void Draw(GameTime gameTime)
    {
        _activeScene?.Draw(gameTime);
    }

    private void TransitionScene(SceneType nextSceneType)
    {
        _activeScene?.UnloadContent();
        _activeScene?.Exit();

        _sceneScope?.Dispose();

        _sceneScope = _scopeFactory.Invoke();

        _activeScene = GetNextScene(nextSceneType, _sceneScope);
        _activeSceneType = nextSceneType;

        _activeScene?.Initialize();
        _activeScene?.LoadContent();
        _activeScene?.Enter();
    }

    private static BaseScene GetNextScene(SceneType sceneType, Scope sceneScope)
    {
        return sceneType switch
        {
            SceneType.Title => sceneScope.GetInstance<TitleScene>(),
            SceneType.Gameplay => sceneScope.GetInstance<GameplayScene>(),
            _ => throw new ArgumentOutOfRangeException(nameof(sceneType), sceneType, null),
        };
    }
}