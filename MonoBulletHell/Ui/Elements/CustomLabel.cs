using Gum.Forms.Controls;
using MonoGameGum.GueDeriving;

namespace MonoBulletHell.Ui.Elements;

public class CustomLabel : Label
{
    public CustomLabel()
    {
        var textRuntime = (TextRuntime)TextComponent;
        textRuntime.UseCustomFont = true;
        textRuntime.CustomFontFile = "fonts/micro_5.fnt";
    }
}