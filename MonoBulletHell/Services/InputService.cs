using Microsoft.Xna.Framework.Input;
using MonoBulletHell.Core.Input;

namespace MonoBulletHell.Services;

public interface IInputService
{
    KeyboardInputInfo Keyboard { get; }

    void Update();

    bool PausePressed();

    bool MoveUp();
    bool MoveDown();
    bool MoveLeft();
    bool MoveRight();

    bool Shoot();
    bool FocusPressed();
}

public class InputService : IInputService
{
    private readonly KeyboardInputInfo _keyboard;

    public KeyboardInputInfo Keyboard => _keyboard;

    public InputService()
    {
        _keyboard = new KeyboardInputInfo();
    }

    public void Update()
    {
        _keyboard.Update();
    }

    public bool PausePressed() => _keyboard.WasKeyJustPressed(Keys.Escape);

    public bool MoveUp() => _keyboard.IsKeyDown(Keys.Up) || _keyboard.IsKeyDown(Keys.W);
    public bool MoveDown() => _keyboard.IsKeyDown(Keys.Down) || _keyboard.IsKeyDown(Keys.S);
    public bool MoveLeft() => _keyboard.IsKeyDown(Keys.Left) || _keyboard.IsKeyDown(Keys.A);
    public bool MoveRight() => _keyboard.IsKeyDown(Keys.Right) || _keyboard.IsKeyDown(Keys.D);

    public bool Shoot() => _keyboard.IsKeyDown(Keys.Space);
    public bool FocusPressed() => _keyboard.IsKeyDown(Keys.LeftShift);
}