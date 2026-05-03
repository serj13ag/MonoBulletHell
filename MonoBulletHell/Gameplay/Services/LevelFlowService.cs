using System;
using MonoBulletHell.Scenes;
using MonoBulletHell.Services;

namespace MonoBulletHell.Gameplay.Services;

public interface ILevelFlowService
{
    void StartLevel();
}

public class LevelFlowService : ILevelFlowService
{
    private readonly ISceneService _sceneService;
    private readonly IEnemySpawnService _enemySpawnService;
    private readonly IEnemyService _enemyService;
    private readonly IBossService _bossService;

    private enum LevelFlowState
    {
        Empty,
        SpawningWaves,
        FightingLastEnemies,
        BossFight,
        Win,
    }

    private LevelFlowState _currentState;

    public LevelFlowService(ISceneService sceneService, IEnemySpawnService enemySpawnService, IEnemyService enemyService,
        IBossService bossService)
    {
        _sceneService = sceneService;
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
        SetState(_bossService.HasBoss ? LevelFlowState.BossFight : LevelFlowState.Win);
    }

    private void OnBossDied()
    {
        SetState(LevelFlowState.Win);
    }

    private void SetState(LevelFlowState newState)
    {
        switch (_currentState)
        {
            case LevelFlowState.Empty:
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
            case LevelFlowState.Win:
            default:
                throw new ArgumentOutOfRangeException(nameof(_currentState), _currentState, null);
        }

        switch (newState)
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
                break;
            case LevelFlowState.Win:
                _sceneService.ChangeScene(SceneType.Title);
                break;
            case LevelFlowState.Empty:
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        _currentState = newState;
    }
}