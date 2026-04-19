using System;
using System.Collections.Generic;
using Gum.Forms.Controls;
using Gum.Wireframe;
using MonoGameGum.GueDeriving;

namespace MonoBulletHell.Ui.Elements.Panels;

public class OptionsPanel : BasePanel
{
    private readonly List<(string, float)> _scales = // TODO: refactor
    [
        ("X1", 1f),
        ("X1.5", 1.5f),
        ("X2", 2f),
        ("X2.5", 2.5f),
    ];

    private readonly IUiMediator _uiMediator;

    private readonly Button _backButton;

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

        var comboBox = new CustomComboBox();
        buttonsPanel.AddChild(comboBox);
        comboBox.Anchor(Gum.Wireframe.Anchor.Top);
        comboBox.Width = 120f;
        foreach (var (scaleString, _) in _scales)
        {
            comboBox.AddItem(scaleString);
        }

        comboBox.SelectedIndex = 0;
        comboBox.SelectionChanged += BoxSelectionChanged;

        _backButton = new CustomButton();
        buttonsPanel.AddChild(_backButton);
        _backButton.Text = UiConstants.BackButtonText;
        _backButton.Click += OnOptionsBackButtonClicked;
    }

    private void OnOptionsBackButtonClicked(object sender, EventArgs e)
    {
        OnBackButtonClicked?.Invoke();
    }

    private void BoxSelectionChanged(object arg1, SelectionChangedEventArgs arg2)
    {
        var box = (ComboBox)arg1;
        var scaleValue = _scales[box.SelectedIndex].Item2;
        _uiMediator.ResolutionScaleSelected(scaleValue);
    }
}