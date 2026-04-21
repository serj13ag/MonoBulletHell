using System;
using Gum.Forms.Controls;
using Gum.Wireframe;
using MonoBulletHell.Enums;
using MonoGameGum.GueDeriving;

namespace MonoBulletHell.Ui.Elements.Panels;

public class OptionsPanel : BasePanel
{
    private readonly IUiMediator _uiMediator;

    private readonly CustomComboBox _comboBox;
    private readonly CustomButton _backButton;

    protected override Button FocusButton => _backButton;

    public event Action OnBackButtonClicked;

    public OptionsPanel(IUiMediator uiMediator)
    {
        _uiMediator = uiMediator;

        Anchor(Gum.Wireframe.Anchor.Center);
        Width = 100f;
        Height = 50f;

        var background = new ColoredRectangleRuntime();
        AddChild(background);
        background.Dock(Gum.Wireframe.Dock.Fill);
        background.Color = Constants.Colors.BackgroundDark;

        var titleText = new CustomLabel();
        AddChild(titleText);
        titleText.Anchor(Gum.Wireframe.Anchor.Top);
        titleText.Text = UiConstants.OptionsButtonText;
        titleText.Y = 10f;

        var buttonsPanel = new StackPanel();
        AddChild(buttonsPanel);
        buttonsPanel.Anchor(Gum.Wireframe.Anchor.Bottom);
        buttonsPanel.Y = -10f;
        buttonsPanel.Spacing = 5f;

        _comboBox = new CustomComboBox();
        buttonsPanel.AddChild(_comboBox);
        _comboBox.Anchor(Gum.Wireframe.Anchor.Top);
        _comboBox.Width = 120f;
        foreach (var scale in uiMediator.GetScreenScales())
        {
            _comboBox.AddItem(FormatScale(scale));
        }

        _comboBox.SelectionChanged += BoxSelectionChanged;

        _backButton = new CustomButton();
        buttonsPanel.AddChild(_backButton);
        _backButton.Text = UiConstants.BackButtonText;
        _backButton.Click += OnOptionsBackButtonClicked;
    }

    public override void Enable()
    {
        base.Enable();

        _comboBox.SelectedIndex = _uiMediator.GetCurrentScaleIndex();
    }

    private void OnOptionsBackButtonClicked(object sender, EventArgs e)
    {
        OnBackButtonClicked?.Invoke();
    }

    private void BoxSelectionChanged(object arg1, SelectionChangedEventArgs arg2)
    {
        var box = (ComboBox)arg1;
        _uiMediator.ResolutionScaleSelected(box.SelectedIndex);
    }

    private static string FormatScale(ScreenScale scale)
    {
        return scale switch
        {
            ScreenScale.X1 => "X1",
            ScreenScale.X1_5 => "X1.5",
            ScreenScale.X2 => "X2",
            ScreenScale.X2_5 => "X2.5",
            ScreenScale.X3 => "X3",
            _ => throw new ArgumentOutOfRangeException(nameof(scale), scale, null),
        };
    }
}