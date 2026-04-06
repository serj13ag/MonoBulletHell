using MonoBulletHell.Gameplay.Entities;

namespace MonoBulletHell.Gameplay;

public interface IGameContext
{
    Ship Ship { get; }
    Enemy Enemy { get; }

    void RegisterShip(Ship ship);
    void RegisterEnemy(Enemy enemy);
}

public class GameContext : IGameContext
{
    public Ship Ship { get; private set; }
    public Enemy Enemy { get; private set; }

    public void RegisterShip(Ship ship)
    {
        Ship = ship;
    }

    public void RegisterEnemy(Enemy enemy)
    {
        Enemy = enemy;
    }
}