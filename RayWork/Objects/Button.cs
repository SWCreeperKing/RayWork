using System.Numerics;
using Raylib_CsLo;
using RayWork.CoreComponents;
using static Raylib_CsLo.MouseButton;
using static Raylib_CsLo.Raylib;

namespace RayWork.Objects;

public class Button : GameObject
{
    public Label label;
    public bool disabled;

    private ColorComponent _panelColor = new();
    private ColorComponent _hoverColor = new();
    private ColorComponent _disabledColor = new();
    private bool _wasDisabled;
    private bool _wasHover;

    public EventHandler OnButtonHover;
    public EventHandler OnButtonPressed;

    public Button(Label label, Color? panelColor = null, Color? hoverColor = null, Color? disabledColor = null)
    {
        AddChild(this.label = label);
        SetColor(panelColor, hoverColor, disabledColor);
        label.panelComponent.panelColor = _panelColor.color;
    }

    public Button(string text, Vector2 position, Color? color = null) : this(new Label(text, position), color)
    {
    }

    public void SetColor(Color? panelColor = null, Color? hoverColor = null, Color? disabledColor = null)
    {
        var nonNullPanelColor = panelColor ?? new Color(80, 100, 160, 255);
        _panelColor.color = nonNullPanelColor;
        _hoverColor.color = hoverColor ?? nonNullPanelColor.MakeLighter();
        _disabledColor.color = disabledColor ?? nonNullPanelColor.MakeDarker();
    }

    public override void UpdateLoop()
    {
        if (disabled)
        {
            if (!_wasDisabled)
            {
                if (_wasHover) _wasHover = false;
                _wasDisabled = true;
                label.panelComponent.panelColor = _disabledColor.color;
            }
        }
        else if (label.rectangle.IsMouseIn())
        {
            if (!_wasHover)
            {
                if (_wasDisabled) _wasDisabled = false;
                _wasHover = true;

                if (OnButtonHover is not null) OnButtonHover(null, null);
                label.panelComponent.panelColor = _hoverColor.color;
            }
        }
        else if (_wasDisabled || _wasHover)
        {
            if (_wasHover) _wasHover = false;
            if (_wasDisabled) _wasDisabled = false;
            label.panelComponent.panelColor = _panelColor.color;
        }

        if (!_wasHover || !IsMouseButtonPressed(MOUSE_BUTTON_LEFT) ||
            OnButtonPressed is null) return;
        OnButtonPressed(null, null);
    }

    public override void RenderLoop()
    {
        label.Render();
    }
}