using Microsoft.Xna.Framework;

namespace MonoBulletHell.Gameplay.Services;

public interface ITimeService
{
    float DeltaTime { get; }

    void Update(GameTime gameTime);
}

public class TimeService : ITimeService
{
    private float _deltaTime;

    public float DeltaTime => _deltaTime;

    public void Update(GameTime gameTime)
    {
        _deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
}