using System.Collections.Generic;
using MonoBulletHell.Gameplay.Entities;
using MonoBulletHell.Gameplay.Factories;

namespace MonoBulletHell.Gameplay.Services;

public interface IEnemyService
{
    void Update();
    void Render(IRenderService renderService);

    void SpawnEnemy(string pathName);
    void Clear();
}

public class EnemyService : IEnemyService
{
    private readonly IContentService _contentService;
    private readonly IGameFactory _gameFactory;
    private readonly IGameContext _context;

    private readonly List<Enemy> _enemiesToDestroy = new List<Enemy>();

    public EnemyService(IContentService contentService, IGameFactory gameFactory, IGameContext context)
    {
        _contentService = contentService;
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

    public void SpawnEnemy(string pathName)
    {
        var path = _contentService.GetPath(pathName);
        var enemy = _gameFactory.CreateEnemy(path);
        _context.Enemies.Add(enemy);
    }

    public void Clear()
    {
        _enemiesToDestroy.Clear();
        _context.Enemies.Clear();
    }
}