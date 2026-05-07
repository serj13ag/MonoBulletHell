using Gum.Forms.Controls;
using Gum.Forms.DefaultVisuals.V3;
using Microsoft.Xna.Framework;

namespace MonoBulletHell.Ui.Elements;

public class CustomButton : Button
{
    private readonly IUiMediator _uiMediator;

    public CustomButton(IUiMediator uiMediator, Color color)
    {
        _uiMediator = uiMediator;

        var buttonVisual = (ButtonVisual)Visual;
        buttonVisual.BackgroundColor = color;

        var textComponent = buttonVisual.TextInstance;
        textComponent.UseCustomFont = true;
        textComponent.CustomFontFile = UiConstants.FontPath;
        textComponent.Text = UiConstants.GameTitleText;
    }

    protected override void OnClick()
    {
        base.OnClick();

        _uiMediator.ButtonClicked();
    }
}