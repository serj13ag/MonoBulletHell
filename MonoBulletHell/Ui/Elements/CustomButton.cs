using Gum.Forms.Controls;
using Gum.Forms.DefaultVisuals.V3;

namespace MonoBulletHell.Ui.Elements;

public class CustomButton : Button
{
    public CustomButton()
    {
        var buttonVisual = (ButtonVisual)Visual;
        buttonVisual.BackgroundColor = Constants.Colors.BackgroundHighlight;

        var textComponent = buttonVisual.TextInstance;
        textComponent.UseCustomFont = true;
        textComponent.CustomFontFile = UiConstants.FontPath;
        textComponent.Text = UiConstants.GameTitleText;
    }
}