using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Enums;

namespace MonoBulletHell.Services;

public interface IScreenService
{
    void Initialize();

    void SetNativeRenderTarget();
    void RenderResultImage();

    Matrix GetTransformMatrix();
}

public class ScreenService : IScreenService, IDisposable
{
    private readonly GraphicsDeviceManager _graphicsDeviceManager;
    private readonly GraphicsDevice _graphicsDevice;
    private readonly SpriteBatch _spriteBatch;
    private readonly ISettingsService _settingsService;

    private RenderTarget2D _nativeRenderTarget;
    private Rectangle _actualScreenRectangle;

    public ScreenService(GraphicsDeviceManager graphicsDeviceManager, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch,
        ISettingsService settingsService)
    {
        _graphicsDeviceManager = graphicsDeviceManager;
        _graphicsDevice = graphicsDevice;
        _spriteBatch = spriteBatch;
        _settingsService = settingsService;
    }

    public void Initialize()
    {
        _nativeRenderTarget = new RenderTarget2D(_graphicsDevice, MonoBulletHellGame.VirtualWidth, MonoBulletHellGame.VirtualHeight);

        _graphicsDeviceManager.IsFullScreen = false;
        ApplyScale(_settingsService.ScreenScale);

        _settingsService.ScreenScaleChanged += OnScreenScaleChanged;
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

    public Matrix GetTransformMatrix()
    {
        var translation = Matrix.CreateTranslation(-_actualScreenRectangle.X, -_actualScreenRectangle.Y, 0);

        var xScale = MonoBulletHellGame.VirtualWidth / (float)_actualScreenRectangle.Width;
        var yScale = MonoBulletHellGame.VirtualHeight / (float)_actualScreenRectangle.Height;
        var scale = Matrix.CreateScale(xScale, yScale, 1f);

        return translation * scale;
    }

    private void OnScreenScaleChanged(ScreenScale screenScale)
    {
        ApplyScale(screenScale);
    }

    private void ApplyScale(ScreenScale scale)
    {
        var scaleValue = GetScaleValue(scale);

        var newWidth = (int)(MonoBulletHellGame.VirtualWidth * scaleValue);
        var newHeight = (int)(MonoBulletHellGame.VirtualHeight * scaleValue);

        _graphicsDeviceManager.PreferredBackBufferWidth = newWidth;
        _graphicsDeviceManager.PreferredBackBufferHeight = newHeight;
        _graphicsDeviceManager.ApplyChanges();

        _actualScreenRectangle = new Rectangle(0, 0, newWidth, newHeight);
    }

    private static float GetScaleValue(ScreenScale scale)
    {
        return scale switch
        {
            ScreenScale.X1 => 1f,
            ScreenScale.X1_5 => 1.5f,
            ScreenScale.X2 => 2f,
            ScreenScale.X2_5 => 2.5f,
            ScreenScale.X3 => 3f,
            _ => throw new ArgumentOutOfRangeException(nameof(scale), scale, null),
        };
    }

    public void Dispose()
    {
        _settingsService.ScreenScaleChanged -= OnScreenScaleChanged;
    }
}