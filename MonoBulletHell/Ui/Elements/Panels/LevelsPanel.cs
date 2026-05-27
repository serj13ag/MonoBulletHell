using System;
using Gum.Forms.Controls;
using Microsoft.Xna.Framework;
using MonoGameGum.GueDeriving;

namespace MonoBulletHell.Ui.Elements.Panels;

public class LevelsPanel : BasePanel
{
    private readonly CustomButton _firstLevelButton;

    protected override Button FocusButton => _firstLevelButton;

    public event Action<int> OnLevelButtonClicked;
    public event Action OnBackButtonClicked;

    public LevelsPanel(UiFactory uiFactory, Color color, int levelsCount)
    {
        Anchor(Gum.Wireframe.Anchor.Center);
        Width = 100f;
        Height = 50f;

        var background = new ColoredRectangleRuntime();
        AddChild(background);
        background.Dock(Gum.Wireframe.Dock.Fill);
        background.Color = color;

        var titleText = new CustomLabel();
        AddChild(titleText);
        titleText.Anchor(Gum.Wireframe.Anchor.Top);
        titleText.Text = UiConstants.LevelsPanelTitleText;
        titleText.Y = 10f;

        var buttonsPanel = new StackPanel();
        AddChild(buttonsPanel);
        buttonsPanel.Anchor(Gum.Wireframe.Anchor.Bottom);
        buttonsPanel.Y = -10f;
        buttonsPanel.Spacing = 5f;

        for (var i = 0; i < levelsCount; i++)
        {
            var button = uiFactory.CreateCustomButton();
            buttonsPanel.AddChild(button);
            button.Text = $"{UiConstants.LevelText} {i + 1}";

            var index = i;
            button.Click += (_, _) => OnLevelButtonClicked?.Invoke(index);

            if (i == 0)
            {
                _firstLevelButton = button;
            }
        }

        var backButton = uiFactory.CreateCustomButton();
        buttonsPanel.AddChild(backButton);
        backButton.Text = UiConstants.BackButtonText;
        backButton.Click += (_, _) => OnBackButtonClicked?.Invoke();
    }
}