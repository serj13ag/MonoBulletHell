using Microsoft.Xna.Framework.Input;

namespace MonoBulletHell.Core.Input;

public class KeyboardInputInfo
{
    private KeyboardState _previousState;
    private KeyboardState _currentState;

    public KeyboardInputInfo()
    {
        _previousState = new KeyboardState();
        _currentState = Keyboard.GetState();
    }

    public void Update()
    {
        _previousState = _currentState;
        _currentState = Keyboard.GetState();
    }

    public bool IsKeyDown(Keys key)
    {
        return _currentState.IsKeyDown(key);
    }

    public bool IsKeyUp(Keys key)
    {
        return _currentState.IsKeyUp(key);
    }

    public bool WasKeyJustPressed(Keys key)
    {
        return _currentState.IsKeyDown(key) && _previousState.IsKeyUp(key);
    }

    public bool WasKeyJustReleased(Keys key)
    {
        return _currentState.IsKeyUp(key) && _previousState.IsKeyDown(key);
    }
}