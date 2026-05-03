using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoBulletHell.Enums;
using MonoBulletHell.Gameplay.Entities;
using MonoBulletHell.Gameplay.Factories;
using MonoBulletHell.Services;

namespace MonoBulletHell.Gameplay.Services;

public interface IEnemyService
{
    event Action AllEnemiesDied;

    void Update();
    void Render(IRenderService renderService);

    void SpawnEnemy(Vector2 position, string pathName, string enemyName, string emitterName);
    Enemy SpawnBoss(string enemyName, Vector2 position, string pathName, string emitterName);
    void Clear();
}

public class EnemyService : IEnemyService
{
    private readonly IGameFactory _gameFactory;
    private readonly IGameContext _context;
    private readonly ISoundService _soundService;

    public event Action AllEnemiesDied;

    public EnemyService(IGameFactory gameFactory, IGameContext context, ISoundService soundService)
    {
        _gameFactory = gameFactory;
        _context = context;
        _soundService = soundService;
    }

    public void Update()
    {
        var enemyDied = false;

        var enemies = _context.Enemies;
        for (var i = enemies.Count - 1; i >= 0; i--)
        {
            var enemy = enemies[i];
            enemy.Update();

            if (enemy.IsDead)
            {
                enemyDied = true;
                RemoveEnemyAt(enemies, i);
            }
            else if (enemy.PathIsFinished)
            {
                RemoveEnemyAt(enemies, i);
            }
        }

        if (enemyDied)
        {
            _soundService.PlaySoundEffect(SfxType.EnemyDied);
        }
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

    public Enemy SpawnBoss(string enemyName, Vector2 position, string pathName, string emitterName)
    {
        var boss = _gameFactory.CreateEnemy(position, pathName, enemyName, emitterName);
        _context.Enemies.Add(boss);
        return boss;
    }

    public void Clear()
    {
        _context.Enemies.Clear();
    }

    private void RemoveEnemyAt(List<Enemy> enemies, int index)
    {
        enemies.RemoveAt(index);
        if (enemies.Count == 0)
        {
            AllEnemiesDied?.Invoke();
        }
    }
}