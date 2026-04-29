using Microsoft.Xna.Framework;
using MonoBulletHell.Enums;
using MonoBulletHell.Gameplay.Factories;
using MonoBulletHell.Services;

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
    private readonly ISoundService _soundService;

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
                enemies.RemoveAt(i);
            }
            else if (enemy.PathIsFinished)
            {
                enemies.RemoveAt(i);
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

    public void Clear()
    {
        _context.Enemies.Clear();
    }
}