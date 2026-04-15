using Microsoft.Xna.Framework;

namespace MonoBulletHell.Gameplay.Services;

public interface ITimeService
{
    GameTime DeltaGameTime { get; }
    float DeltaTime { get; }

    void Update(GameTime gameTime);
}

public class TimeService : ITimeService
{
    public GameTime DeltaGameTime { get; private set; }
    public float DeltaTime { get; private set; }

    public void Update(GameTime gameTime)
    {
        DeltaGameTime = gameTime;
        DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
}