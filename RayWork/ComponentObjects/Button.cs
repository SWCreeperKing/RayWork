using System.Numerics;
using Raylib_CsLo;
using RayWork.CoreComponents;
using static Raylib_CsLo.MouseButton;
using static Raylib_CsLo.Raylib;

namespace RayWork.Objects;

public class Button : GameObject
{
    public Label Label;
    public bool Disabled;

    private ColorComponent PanelColor = new();
    private ColorComponent HoverColor = new();
    private ColorComponent DisabledColor = new();
    private bool WasDisabled;
    private bool WasHover;

    public EventHandler OnButtonHover;
    public EventHandler OnButtonPressed;

    public Button(Label label, Color? panelColor = null, Color? hoverColor = null, Color? disabledColor = null)
    {
        AddChild(Label = label);
        SetColor(panelColor, hoverColor, disabledColor);
        label.PanelComponent.PanelColor = PanelColor.Color;
    }

    public Button(string text, Vector2 position, Color? color = null) : this(new(text, position), color)
    {
    }

    public void SetColor(Color? panelColor = null, Color? hoverColor = null, Color? disabledColor = null)
    {
        var nonNullPanelColor = panelColor ?? new Color(80, 100, 160, 255);
        PanelColor.Color = nonNullPanelColor;
        HoverColor.Color = hoverColor ?? nonNullPanelColor.MakeLighter();
        DisabledColor.Color = disabledColor ?? nonNullPanelColor.MakeDarker();
    }

    public override void UpdateLoop()
    {
        if (Disabled)
        {
            if (!WasDisabled)
            {
                if (WasHover)
                {
                    WasHover = false;
                }

                WasDisabled = true;
                Label.PanelComponent.PanelColor = DisabledColor.Color;
            }
        }
        else if (Label.Rectangle.IsMouseIn())
        {
            if (!WasHover)
            {
                if (WasDisabled)
                {
                    WasDisabled = false;
                }

                WasHover = true;

                if (OnButtonHover is not null)
                {
                    OnButtonHover(null, null);
                }

                Label.PanelComponent.PanelColor = HoverColor.Color;
            }
        }
        else if (WasDisabled || WasHover)
        {
            if (WasHover)
            {
                WasHover = false;
            }

            if (WasDisabled)
            {
                WasDisabled = false;
            }

            Label.PanelComponent.PanelColor = PanelColor.Color;
        }

        if (!WasHover || !IsMouseButtonPressed(MOUSE_BUTTON_LEFT) ||
            OnButtonPressed is null) return;
        OnButtonPressed(null, null);
    }

    public override void RenderLoop() => Label.Render();
}