using Microsoft.Xna.Framework.Input;
using MonoBulletHell.Core.Input;

namespace MonoBulletHell.Services;

public class InputService
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
}