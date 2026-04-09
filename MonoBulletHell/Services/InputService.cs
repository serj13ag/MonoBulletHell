using Microsoft.Xna.Framework.Input;
using MonoBulletHell.Core.Input;

namespace MonoBulletHell.Services;

public interface IInputService
{
    KeyboardInputInfo Keyboard { get; }

    void Update();

    void SetExitOnEscapeKeyPressed(bool value);

    bool PausePressed();

    bool MoveUp();
    bool MoveDown();
    bool MoveLeft();
    bool MoveRight();

    bool Shoot();
}

public class InputService : IInputService
{
    private readonly IGameService _gameService;

    private readonly KeyboardInputInfo _keyboard;

    private bool _exitOnEscape;

    public KeyboardInputInfo Keyboard => _keyboard;

    public InputService(IGameService gameService)
    {
        _gameService = gameService;

        _keyboard = new KeyboardInputInfo();
    }

    public void Update()
    {
        _keyboard.Update();

        if (_exitOnEscape && _keyboard.WasKeyJustPressed(Keys.Escape))
        {
            _gameService.Exit();
        }
    }

    public void SetExitOnEscapeKeyPressed(bool value)
    {
        _exitOnEscape = value;
    }

    public bool PausePressed()
    {
        return _keyboard.WasKeyJustPressed(Keys.Escape);
    }

    public bool MoveUp()
    {
        return _keyboard.IsKeyDown(Keys.Up) ||
               _keyboard.IsKeyDown(Keys.W);
    }

    public bool MoveDown()
    {
        return _keyboard.IsKeyDown(Keys.Down) ||
               _keyboard.IsKeyDown(Keys.S);
    }

    public bool MoveLeft()
    {
        return _keyboard.IsKeyDown(Keys.Left) ||
               _keyboard.IsKeyDown(Keys.A);
    }

    public bool MoveRight()
    {
        return _keyboard.IsKeyDown(Keys.Right) ||
               _keyboard.IsKeyDown(Keys.D);
    }

    public bool Shoot()
    {
        return _keyboard.IsKeyDown(Keys.Space);
    }
}