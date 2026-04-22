using Microsoft.Xna.Framework.Graphics;

namespace MonoBulletHell.Gameplay.Services;

public interface IBackgroundService
{
    void Initialize(Texture2D backgroundTexture);
    void Update();
    void Render(IRenderService renderService);
}

public class BackgroundService : IBackgroundService
{
    private const float ScrollSpeed = 50f;

    private readonly ITimeService _timeService;

    private Texture2D _backgroundTexture;
    private float _backgroundVerticalOffset;

    public BackgroundService(ITimeService timeService)
    {
        _timeService = timeService;
    }

    public void Initialize(Texture2D backgroundTexture)
    {
        _backgroundTexture = backgroundTexture;
        _backgroundVerticalOffset = 0f;
    }

    public void Update()
    {
        var offset = ScrollSpeed * _timeService.DeltaTime;
        _backgroundVerticalOffset -= offset;

        // Wrap texture
        _backgroundVerticalOffset %= _backgroundTexture.Height;
    }

    public void Render(IRenderService renderService)
    {
        renderService.SetBackgroundBatch(_backgroundTexture, (int)_backgroundVerticalOffset);
    }
}