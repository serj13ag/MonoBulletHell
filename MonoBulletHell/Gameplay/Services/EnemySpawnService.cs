using System.Collections.Generic;
using MonoBulletHell.Data;
using MonoBulletHell.Helpers;

namespace MonoBulletHell.Gameplay.Services;

public interface IEnemySpawnService
{
    void Initialize(SpawnConfig spawnConfig);

    void Update();
}

public class EnemySpawnService : IEnemySpawnService
{
    private readonly ITimeService _timeService;
    private readonly IEnemyService _enemyService;

    private readonly Queue<WaveData> _waves = new Queue<WaveData>();

    private float _passedTime;
    private WaveData _nextWaveToSpawn;

    public EnemySpawnService(ITimeService timeService, IEnemyService enemyService)
    {
        _timeService = timeService;
        _enemyService = enemyService;
    }

    public void Initialize(SpawnConfig spawnConfig)
    {
        _passedTime = 0f;

        _waves.Clear();
        foreach (var waveData in spawnConfig.Waves)
        {
            _waves.Enqueue(waveData);
        }

        TrySetNextWave();
    }

    public void Update()
    {
        if (_nextWaveToSpawn == null)
        {
            return;
        }

        _passedTime += _timeService.DeltaTime;

        if (_passedTime > _nextWaveToSpawn.SpawnTime)
        {
            SpawnWave(_nextWaveToSpawn);

            TrySetNextWave();
        }
    }

    private void SpawnWave(WaveData nextWaveToSpawn)
    {
        if (nextWaveToSpawn.Formation == null)
        {
            _enemyService.SpawnEnemy(nextWaveToSpawn.Position, nextWaveToSpawn.PathName);
            return;
        }

        foreach (var spawnPosition in FormationHelper.GetSpawnPositions(nextWaveToSpawn.Formation, nextWaveToSpawn.Position))
        {
            _enemyService.SpawnEnemy(spawnPosition, nextWaveToSpawn.PathName);
        }
    }

    private void TrySetNextWave()
    {
        _nextWaveToSpawn = _waves.Count > 0 ? _waves.Dequeue() : null;
    }
}