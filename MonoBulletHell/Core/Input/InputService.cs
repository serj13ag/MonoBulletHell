using Microsoft.Xna.Framework.Input;

namespace MonoBulletHell.Core.Input;

public interface IInputService
{
    KeyboardInputInfo Keyboard { get; }

    void Update();

    bool CancelWasJustPressed();
    bool DebugWasJustPressed();
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

    public bool CancelWasJustPressed() => _keyboard.WasKeyJustPressed(Keys.Escape);
    public bool DebugWasJustPressed() => _keyboard.WasKeyJustPressed(Keys.F1);
}