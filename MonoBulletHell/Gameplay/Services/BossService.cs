using System;
using System.Collections.Generic;
using MonoBulletHell.Data;
using MonoBulletHell.Gameplay.Entities;
using MonoBulletHell.Gameplay.Factories;

namespace MonoBulletHell.Gameplay.Services;

public interface IBossService
{
    void Initialize(SpawnConfig spawnConfig);
}

public class BossService : IBossService, IDisposable
{
    private readonly IEnemySpawnService _enemySpawnService;
    private readonly IGameFactory _gameFactory;
    private readonly IEnemyService _enemyService;

    private readonly Queue<BossStageData> _stages = new Queue<BossStageData>();

    private BossData _bossData;

    private Enemy _boss;
    private BossStageData _nextStage;

    public BossService(IEnemySpawnService enemySpawnService, IGameFactory gameFactory, IEnemyService enemyService)
    {
        _enemySpawnService = enemySpawnService;
        _gameFactory = gameFactory;
        _enemyService = enemyService;
    }

    public void Initialize(SpawnConfig spawnConfig)
    {
        _bossData = spawnConfig.Boss;
        foreach (var bossStage in spawnConfig.Boss.Stages)
        {
            _stages.Enqueue(bossStage);
        }

        _enemySpawnService.LastWaveSpawned += OnAllWavesSpawned;
    }

    private void OnAllWavesSpawned()
    {
        var firstStage = _stages.Dequeue();
        _nextStage = _stages.Dequeue();

        _boss = _enemyService.SpawnBoss(_bossData.EnemyName, _bossData.Position, firstStage.PathName, firstStage.EmitterName);

        _boss.HealthChanged += OnBossHealthChanged;
    }

    private void OnBossHealthChanged(int newHealth)
    {
        if (_nextStage == null)
        {
            return;
        }

        var healthPercent = newHealth / (float)_boss.Health;
        if (healthPercent <= _nextStage.HealthPercent)
        {
            var pathBlock = _gameFactory.CreatePathBlock(_nextStage.PathName, _boss.Position);
            _boss.ChangePathBlock(pathBlock);

            var emitter = _gameFactory.CreateEmitter(_nextStage.EmitterName);
            _boss.ChangeEmitter(emitter);

            _nextStage = _stages.Count > 0 ? _stages.Dequeue() : null;
        }
    }

    public void Dispose()
    {
        _enemySpawnService.LastWaveSpawned -= OnAllWavesSpawned;

        if (_boss != null)
        {
            _boss.HealthChanged -= OnBossHealthChanged;
        }
    }
}