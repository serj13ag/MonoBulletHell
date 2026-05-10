using System;
using System.Collections.Generic;
using MonoBulletHell.Data.Configs;
using MonoBulletHell.Data.DTOs;
using MonoBulletHell.Gameplay.Entities;
using MonoBulletHell.Gameplay.Factories;

namespace MonoBulletHell.Gameplay.Services;

public interface IBossService
{
    bool HasBoss { get; }
    Enemy Boss { get; }

    event Action BossDied;

    void Initialize(LevelConfig levelConfig);
    void SpawnBoss();
}

public class BossService : IBossService, IDisposable
{
    private readonly IGameFactory _gameFactory;
    private readonly IEnemyService _enemyService;

    private readonly Queue<BossStageData> _stages = new Queue<BossStageData>();

    private BossData _bossData;

    private Enemy _boss;
    private BossStageData _nextStage;

    public bool HasBoss => _bossData != null;
    public Enemy Boss => _boss;

    public event Action BossDied;

    public BossService(IGameFactory gameFactory, IEnemyService enemyService)
    {
        _gameFactory = gameFactory;
        _enemyService = enemyService;
    }

    public void Initialize(LevelConfig levelConfig)
    {
        _bossData = null;
        _stages.Clear();

        if (levelConfig.Boss != null)
        {
            _bossData = levelConfig.Boss;
            foreach (var bossStage in levelConfig.Boss.Stages)
            {
                _stages.Enqueue(bossStage);
            }
        }
    }

    public void SpawnBoss()
    {
        var firstStage = _stages.Dequeue();
        SetNextStage();

        _boss = _enemyService.SpawnBoss(_bossData.EnemyName, _bossData.Position, firstStage.PathName, firstStage.EmitterName);

        _boss.HealthChanged += OnBossHealthChanged;
    }

    private void OnBossHealthChanged(HealthChangedDTO healthChangedDto)
    {
        if (healthChangedDto.NewHealth <= 0)
        {
            BossDied?.Invoke();
            return;
        }

        if (_nextStage == null)
        {
            return;
        }

        var healthPercent = healthChangedDto.NewHealth / (float)healthChangedDto.MaxHealth;
        if (healthPercent <= _nextStage.HealthPercent)
        {
            var movement = _gameFactory.CreateMovement(_nextStage.PathName, _boss.Position);
            _boss.ChangeMovement(movement);

            var emitter = _gameFactory.CreateEmitter(_nextStage.EmitterName);
            _boss.ChangeEmitter(emitter);

            SetNextStage();
        }
    }

    private void SetNextStage()
    {
        _nextStage = _stages.Count > 0 ? _stages.Dequeue() : null;
    }

    public void Dispose()
    {
        if (_boss != null)
        {
            _boss.HealthChanged -= OnBossHealthChanged;
        }
    }
}