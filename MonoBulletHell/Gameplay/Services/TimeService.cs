using Microsoft.Xna.Framework;

namespace MonoBulletHell.Gameplay.Services;

public class TimeService
{
    private float _deltaTime;

    public float DeltaTime => _deltaTime;

    public void Update(GameTime gameTime)
    {
        _deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
}