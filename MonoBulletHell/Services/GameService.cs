namespace MonoBulletHell.Services;

public interface IGameService
{
    void ApplyScale(float scale);
    void Exit();
}

public class GameService : IGameService
{
    private readonly MonoBulletHellGame _game;

    public GameService(MonoBulletHellGame game)
    {
        _game = game;
    }

    public void ApplyScale(float scale)
    {
        _game.ApplyScale(scale);
    }

    public void Exit()
    {
        _game.Exit();
    }
}