using Microsoft.Xna.Framework.Input;
using MonoBulletHell.Core.Input;

namespace MonoBulletHell.Gameplay.Services;

public interface IInputActionService
{
    bool MoveUp();
    bool MoveDown();
    bool MoveLeft();
    bool MoveRight();

    bool Shoot();
    bool Focus();
}

public class InputActionService : IInputActionService
{
    private readonly IInputService _inputService;

    public InputActionService(IInputService inputService)
    {
        _inputService = inputService;
    }

    public bool MoveUp() => _inputService.Keyboard.IsKeyDown(Keys.Up) || _inputService.Keyboard.IsKeyDown(Keys.W);
    public bool MoveDown() => _inputService.Keyboard.IsKeyDown(Keys.Down) || _inputService.Keyboard.IsKeyDown(Keys.S);
    public bool MoveLeft() => _inputService.Keyboard.IsKeyDown(Keys.Left) || _inputService.Keyboard.IsKeyDown(Keys.A);
    public bool MoveRight() => _inputService.Keyboard.IsKeyDown(Keys.Right) || _inputService.Keyboard.IsKeyDown(Keys.D);

    public bool Shoot() => _inputService.Keyboard.IsKeyDown(Keys.Space);
    public bool Focus() => _inputService.Keyboard.IsKeyDown(Keys.LeftShift);
}