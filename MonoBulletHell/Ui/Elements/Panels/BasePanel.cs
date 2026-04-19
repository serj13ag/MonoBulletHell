using Gum.Forms.Controls;

namespace MonoBulletHell.Ui.Elements.Panels;

public abstract class BasePanel : Panel
{
    protected abstract Button FocusButton { get; }

    public void Enable()
    {
        IsVisible = true;

        if (FocusButton != null)
        {
            FocusButton.IsFocused = true;
        }
    }

    public void Disable()
    {
        IsVisible = false;

        if (FocusButton != null)
        {
            FocusButton.IsFocused = false;
        }
    }
}