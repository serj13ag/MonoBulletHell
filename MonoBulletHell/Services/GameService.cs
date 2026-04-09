using Microsoft.Xna.Framework;

namespace MonoBulletHell.Services;

public interface IGameService
{
    void Exit();
}

public class GameService : IGameService
{
    private readonly Game _game;

    public GameService(Game game)
    {
        _game = game;
    }

    public void Exit()
    {
        _game.Exit();
    }
}