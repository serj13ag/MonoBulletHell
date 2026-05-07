using System;
using LightInject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Core.Input;
using MonoBulletHell.Gameplay;
using MonoBulletHell.Gameplay.Factories;
using MonoBulletHell.Gameplay.Services;
using MonoBulletHell.Scenes;
using MonoBulletHell.Services;
using MonoBulletHell.Ui;
using MonoGameGum;

namespace MonoBulletHell;

public class CompositionRoot
{
    private readonly ServiceContainer _container;

    public CompositionRoot(MonoBulletHellGame game)
    {
        _container = new ServiceContainer();

        RegisterGlobal(game);
        RegisterScenes();
        RegisterGameplay();
    }

    public void Initialize(ContentManager contentManager, GraphicsDevice graphicsDevice)
    {
        _container.RegisterInstance(contentManager);
        _container.RegisterInstance(graphicsDevice);
        _container.RegisterInstance(new SpriteBatch(graphicsDevice));
    }

    public T GetInstance<T>()
    {
        return _container.GetInstance<T>();
    }

    private void RegisterGlobal(MonoBulletHellGame game)
    {
        _container.RegisterInstance(new GraphicsDeviceManager(game));

        _container.RegisterInstance(GumService.Default);

        _container.RegisterInstance<IGameService>(new GameService(game));

        _container.Register<IInputService, InputService>(new PerContainerLifetime());
        _container.Register<IScreenService, ScreenService>(new PerContainerLifetime());
        _container.Register<ISceneService, SceneService>(new PerContainerLifetime());
        _container.Register<IDebugService, DebugService>(new PerContainerLifetime());
        _container.Register<ISettingsService, SettingsService>(new PerContainerLifetime());
        _container.Register<ISoundService, SoundService>(new PerContainerLifetime());
        _container.Register<ISaveService, SaveService>(new PerContainerLifetime());
        _container.Register<IContentService, ContentService>(new PerContainerLifetime());

        _container.Register<IUiMediator, UiMediator>(new PerContainerLifetime());
        _container.Register<IUiFactory, UiFactory>(new PerContainerLifetime());

        // Scope factory
        _container.Register<Func<Scope>>(c => c.BeginScope);
    }

    private void RegisterScenes()
    {
        _container.Register<GameplayScene>(new PerScopeLifetime());
        _container.Register<TitleScene>(new PerScopeLifetime());
    }

    private void RegisterGameplay()
    {
        _container.Register<IInputActionService, InputActionService>(new PerScopeLifetime());
        _container.Register<ITimeService, TimeService>(new PerScopeLifetime());
        _container.Register<ILevelFlowService, LevelFlowService>(new PerScopeLifetime());
        _container.Register<IEnemyService, EnemyService>(new PerScopeLifetime());
        _container.Register<IEnemySpawnService, EnemySpawnService>(new PerScopeLifetime());
        _container.Register<IBossService, BossService>(new PerScopeLifetime());
        _container.Register<IBulletService, BulletService>(new PerScopeLifetime());
        _container.Register<IBackgroundService, BackgroundService>(new PerScopeLifetime());
        _container.Register<IParticleService, ParticleService>(new PerScopeLifetime());
        _container.Register<IRenderService, RenderService>(new PerScopeLifetime());

        _container.Register<IGameContext, GameContext>(new PerScopeLifetime());

        _container.Register<IGameFactory, GameFactory>(new PerScopeLifetime());
        _container.Register<IBulletFactory, BulletFactory>(new PerScopeLifetime());
    }
}