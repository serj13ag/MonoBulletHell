using System;
using LightInject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Gameplay;
using MonoBulletHell.Gameplay.Factories;
using MonoBulletHell.Gameplay.Services;
using MonoBulletHell.Scenes;
using MonoBulletHell.Services;
using MonoGameGum;

namespace MonoBulletHell;

public class CompositionRoot
{
    private readonly ServiceContainer _container;

    public CompositionRoot(Game game)
    {
        _container = new ServiceContainer();

        _container.RegisterInstance(game); // TODO: create service?
        RegisterGlobal();
        RegisterGameplay();
    }

    public void Initialize(ContentManager contentManager, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        _container.RegisterInstance(contentManager);
        _container.RegisterInstance(graphicsDevice);
        _container.RegisterInstance(spriteBatch);
    }

    public T GetInstance<T>()
    {
        return _container.GetInstance<T>();
    }

    private void RegisterGlobal()
    {
        _container.RegisterInstance(GumService.Default);

        _container.Register<IInputService, InputService>(new PerContainerLifetime());
        _container.Register<ISceneService, SceneService>(new PerContainerLifetime());
        _container.Register<IDebugService, DebugService>(new PerContainerLifetime());

        // Scope factory
        _container.Register<Func<Scope>>(c => c.BeginScope);
    }

    private void RegisterGameplay()
    {
        _container.Register<GameplayScene>(new PerScopeLifetime());
        _container.Register<TitleScene>(new PerScopeLifetime());

        _container.Register<IContentService, ContentService>(new PerScopeLifetime());
        _container.Register<ITimeService, TimeService>(new PerScopeLifetime());
        _container.Register<IBulletService, BulletService>(new PerScopeLifetime());

        _container.Register<IGameContext, GameContext>(new PerScopeLifetime());

        _container.Register<IGameFactory, GameFactory>(new PerScopeLifetime());
        _container.Register<IBulletFactory, BulletFactory>(new PerScopeLifetime());
    }
}