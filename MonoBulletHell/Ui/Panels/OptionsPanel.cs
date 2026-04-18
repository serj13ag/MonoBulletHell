using System;
using Gum.Forms.Controls;
using Microsoft.Xna.Framework;
using MonoGameGum.GueDeriving;

namespace MonoBulletHell.Ui.Panels;

public class OptionsPanel : BasePanel
{
    private readonly Button _backButton;

    protected override Button FocusButton => _backButton;

    public event Action OnBackButtonClicked;

    public OptionsPanel()
    {
        Anchor(Gum.Wireframe.Anchor.Center);
        Width = 100f;
        Height = 50f;

        var background = new ColoredRectangleRuntime();
        AddChild(background);
        background.Dock(Gum.Wireframe.Dock.Fill);
        background.Color = Color.DarkSlateBlue;

        var titleText = new Label();
        AddChild(titleText);
        titleText.Anchor(Gum.Wireframe.Anchor.Top);
        titleText.Text = UiConstants.OptionsButtonText;
        titleText.Y = 10f;

        var buttonsPanel = new StackPanel();
        AddChild(buttonsPanel);
        buttonsPanel.Anchor(Gum.Wireframe.Anchor.Bottom);
        buttonsPanel.Y = -10f;
        buttonsPanel.Spacing = 5f;

        _backButton = new Button();
        buttonsPanel.AddChild(_backButton);
        _backButton.Text = UiConstants.BackButtonText;
        _backButton.Click += OnOptionsBackButtonClicked;
    }

    private void OnOptionsBackButtonClicked(object sender, EventArgs e)
    {
        OnBackButtonClicked?.Invoke();
    }
}