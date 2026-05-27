namespace MonoBulletHell.Scenes;

public class GameplaySceneArgs
{
    public int LevelIndex { get; }

    public GameplaySceneArgs(int levelIndex)
    {
        LevelIndex = levelIndex;
    }
}