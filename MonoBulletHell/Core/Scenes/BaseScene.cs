using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoBulletHell.Core.Scenes;

public abstract class BaseScene
{
    protected readonly ContentManager Content;
    protected readonly GraphicsDevice GraphicsDevice;
    protected readonly SpriteBatch SpriteBatch;

    protected BaseScene(ContentManager contentManager, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        Content = new ContentManager(contentManager.ServiceProvider);
        Content.RootDirectory = contentManager.RootDirectory;

        GraphicsDevice = graphicsDevice;
        SpriteBatch = spriteBatch;
    }

    public virtual void Initialize()
    {
    }

    public virtual void LoadContent()
    {
    }

    public virtual void Enter()
    {
    }

    public virtual void Update(GameTime gameTime)
    {
    }

    public virtual void LateUpdate(GameTime gameTime)
    {
    }

    public virtual void Draw(GameTime gameTime)
    {
    }

    public virtual void UnloadContent()
    {
        Content.Unload();
    }

    public virtual void Exit()
    {
        Content.Dispose();
    }
}