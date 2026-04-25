using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoBulletHell.Gameplay.Entities;
using MonoBulletHell.Gameplay.Factories;

namespace MonoBulletHell.Gameplay.Services;

public interface IEnemyService
{
    void Update();
    void Render(IRenderService renderService);

    void SpawnEnemy(Vector2 position, string pathName, string enemyName, string emitterName);
    void Clear();
}

public class EnemyService : IEnemyService
{
    private readonly IGameFactory _gameFactory;
    private readonly IGameContext _context;

    private readonly List<Enemy> _enemiesToDestroy = new List<Enemy>();

    public EnemyService(IGameFactory gameFactory, IGameContext context)
    {
        _gameFactory = gameFactory;
        _context = context;
    }

    public void Update()
    {
        foreach (var enemy in _context.Enemies)
        {
            enemy.Update();

            if (enemy.IsDead || enemy.PathIsFinished)
            {
                _enemiesToDestroy.Add(enemy);
            }
        }

        foreach (var enemyToDestroy in _enemiesToDestroy)
        {
            _context.Enemies.Remove(enemyToDestroy);
        }

        _enemiesToDestroy.Clear();
    }

    public void Render(IRenderService renderService)
    {
        foreach (var enemy in _context.Enemies)
        {
            enemy.Render(renderService);
        }
    }

    public void SpawnEnemy(Vector2 position, string pathName, string enemyName, string emitterName)
    {
        var enemy = _gameFactory.CreateEnemy(position, pathName, enemyName, emitterName);
        _context.Enemies.Add(enemy);
    }

    public void Clear()
    {
        _enemiesToDestroy.Clear();
        _context.Enemies.Clear();
    }
}