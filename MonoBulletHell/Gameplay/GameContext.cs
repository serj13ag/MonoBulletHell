using System.Collections.Generic;
using MonoBulletHell.Gameplay.Entities;

namespace MonoBulletHell.Gameplay;

public interface IGameContext
{
    Ship Ship { get; }

    List<Enemy> Enemies { get; }

    void RegisterShip(Ship ship);
}

public class GameContext : IGameContext
{
    public Ship Ship { get; private set; }

    public List<Enemy> Enemies { get; } = new List<Enemy>();

    public void RegisterShip(Ship ship)
    {
        Ship = ship;
    }
}