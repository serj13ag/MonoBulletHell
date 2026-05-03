using System;

namespace MonoBulletHell.Gameplay.Services;

public interface ILevelFlowService
{
    event Action BossFightStarted;
    event Action LevelFinished;

    void StartLevel();
}

public class LevelFlowService : ILevelFlowService
{
    private readonly IEnemySpawnService _enemySpawnService;
    private readonly IEnemyService _enemyService;
    private readonly IBossService _bossService;

    private enum LevelFlowState
    {
        Empty,
        SpawningWaves,
        FightingLastEnemies,
        BossFight,
        Finished,
    }

    private LevelFlowState _currentState;

    public event Action BossFightStarted;
    public event Action LevelFinished;

    public LevelFlowService(IEnemySpawnService enemySpawnService, IEnemyService enemyService, IBossService bossService)
    {
        _enemySpawnService = enemySpawnService;
        _enemyService = enemyService;
        _bossService = bossService;
    }

    public void StartLevel()
    {
        SetState(LevelFlowState.SpawningWaves);
    }

    private void OnLastWaveSpawned()
    {
        SetState(LevelFlowState.FightingLastEnemies);
    }

    private void OnAllEnemiesDied()
    {
        SetState(_bossService.HasBoss ? LevelFlowState.BossFight : LevelFlowState.Finished);
    }

    private void OnBossDied()
    {
        SetState(LevelFlowState.Finished);
    }

    private void SetState(LevelFlowState newState)
    {
        if (_currentState == newState)
        {
            return;
        }

        ExitState(_currentState);
        _currentState = newState;
        EnterState(newState);
    }

    private void EnterState(LevelFlowState state)
    {
        switch (state)
        {
            case LevelFlowState.SpawningWaves:
                _enemySpawnService.LastWaveSpawned += OnLastWaveSpawned;
                break;
            case LevelFlowState.FightingLastEnemies:
                _enemyService.AllEnemiesDied += OnAllEnemiesDied;
                break;
            case LevelFlowState.BossFight:
                _bossService.SpawnBoss();
                _bossService.BossDied += OnBossDied;
                BossFightStarted?.Invoke();
                break;
            case LevelFlowState.Finished:
                LevelFinished?.Invoke();
                break;
            case LevelFlowState.Empty:
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    private void ExitState(LevelFlowState state)
    {
        switch (state)
        {
            case LevelFlowState.Empty:
            case LevelFlowState.Finished:
                break;
            case LevelFlowState.SpawningWaves:
                _enemySpawnService.LastWaveSpawned -= OnLastWaveSpawned;
                break;
            case LevelFlowState.FightingLastEnemies:
                _enemyService.AllEnemiesDied -= OnAllEnemiesDied;
                break;
            case LevelFlowState.BossFight:
                _bossService.BossDied -= OnBossDied;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}