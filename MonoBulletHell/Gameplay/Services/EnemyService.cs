using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Gameplay.Entities;
using MonoBulletHell.Gameplay.Factories;

namespace MonoBulletHell.Gameplay.Services;

public interface IEnemyService
{
    void Update();
    void Draw(SpriteBatch spriteBatch);

    void SpawnEnemy(Vector2 vector2);
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
        foreach (var enemyToDestroy in _enemiesToDestroy)
        {
            _context.Enemies.Remove(enemyToDestroy);
        }

        _enemiesToDestroy.Clear();

        foreach (var enemy in _context.Enemies)
        {
            enemy.Update();
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var enemy in _context.Enemies)
        {
            enemy.Draw(spriteBatch);
        }
    }

    public void SpawnEnemy(Vector2 vector2)
    {
        var enemy = _gameFactory.CreateEnemy(vector2);
        _context.Enemies.Add(enemy);
        enemy.OnDestroyed += OnEnemyDestroyed;
    }

    public void Clear()
    {
        _enemiesToDestroy.Clear();
        _context.Enemies.Clear();
    }

    private void OnEnemyDestroyed(object sender, EventArgs e)
    {
        var enemy = (Enemy)sender;
        _enemiesToDestroy.Add(enemy);
    }
}