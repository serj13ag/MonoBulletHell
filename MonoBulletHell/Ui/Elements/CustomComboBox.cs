using Gum.Forms.Controls;
using Gum.Forms.DefaultVisuals.V3;

namespace MonoBulletHell.Ui.Elements;

public class CustomComboBox : ComboBox
{
    public CustomComboBox()
    {
        var comboBoxVisual = (ComboBoxVisual)Visual;
        var textInstance = comboBoxVisual.TextInstance;
        textInstance.UseCustomFont = true;
        textInstance.CustomFontFile = UiConstants.FontPath;
    }

    public void AddItem(string scaleString)
    {
        ListBoxItem item = new();
        item.UpdateToObject(scaleString);

        var visual = (ListBoxItemVisual)item.Visual;

        var textInstance = visual.TextInstance;
        textInstance.UseCustomFont = true;
        textInstance.CustomFontFile = UiConstants.FontPath;

        Items.Add(item);
    }
}