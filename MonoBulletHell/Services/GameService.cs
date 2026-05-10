using MonoBulletHell.App;

namespace MonoBulletHell.Services;

public interface IGameService
{
    void Exit();
}

public class GameService : IGameService
{
    private readonly MonoBulletHellGame _game;

    public GameService(MonoBulletHellGame game)
    {
        _game = game;
    }

    public void Exit()
    {
        _game.Exit();
    }
}