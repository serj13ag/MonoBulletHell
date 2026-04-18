using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoBulletHell.Services;

public interface IScreenService
{
    void Initialize();

    void SetNativeRenderTarget();
    void RenderResultImage();

    void ApplyScale(float scale);

    Matrix GetTransformMatrix();
}

public class ScreenService : IScreenService
{
    private const float InitialScale = 1f;

    private readonly GraphicsDeviceManager _graphicsDeviceManager;
    private readonly GraphicsDevice _graphicsDevice;
    private readonly SpriteBatch _spriteBatch;

    private RenderTarget2D _nativeRenderTarget;
    private float _scale;
    private Rectangle _actualScreenRectangle;

    public ScreenService(GraphicsDeviceManager graphicsDeviceManager, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        _graphicsDeviceManager = graphicsDeviceManager;
        _graphicsDevice = graphicsDevice;
        _spriteBatch = spriteBatch;
    }

    public void Initialize()
    {
        _nativeRenderTarget = new RenderTarget2D(_graphicsDevice, Constants.VirtualWidth, Constants.VirtualHeight);

        _graphicsDeviceManager.IsFullScreen = false;
        ApplyScale(InitialScale);
    }

    public void SetNativeRenderTarget()
    {
        _graphicsDevice.SetRenderTarget(_nativeRenderTarget);
    }

    public void RenderResultImage()
    {
        _graphicsDevice.SetRenderTarget(null);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _spriteBatch.Draw(_nativeRenderTarget, _actualScreenRectangle, Color.White);
        _spriteBatch.End();
    }

    public void ApplyScale(float scale)
    {
        _scale = scale;

        var newWidth = (int)(Constants.VirtualWidth * scale);
        var newHeight = (int)(Constants.VirtualHeight * scale);

        _graphicsDeviceManager.PreferredBackBufferWidth = newWidth;
        _graphicsDeviceManager.PreferredBackBufferHeight = newHeight;
        _graphicsDeviceManager.ApplyChanges();

        _actualScreenRectangle = new Rectangle(0, 0, newWidth, newHeight);
    }

    public Matrix GetTransformMatrix()
    {
        var translation = Matrix.CreateTranslation(-_actualScreenRectangle.X, -_actualScreenRectangle.Y, 0);

        var xScale = Constants.VirtualWidth / (float)_actualScreenRectangle.Width;
        var yScale = Constants.VirtualHeight / (float)_actualScreenRectangle.Height;
        var scale = Matrix.CreateScale(xScale, yScale, 1f);

        return translation * scale;
    }
}