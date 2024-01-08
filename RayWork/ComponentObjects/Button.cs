using System.Numerics;
using ImGuiNET;
using Raylib_cs;
using RayWork.CoreComponents;
using RayWork.EventArguments;
using RayWork.Objects;
using RayWork.Objects.Primitives;
using static Raylib_cs.MouseButton;
using static Raylib_cs.MouseCursor;

namespace RayWork.ComponentObjects;

public class Button : GameObject
{
    public Label Label;
    public bool Disabled;

    private ColorComponent PanelColor = new();
    private ColorComponent HoverColor = new();
    private ColorComponent DisabledColor = new();
    private bool WasDisabled;
    private bool WasHover;

    public event EventHandler<bool>? OnButtonHoveringChanged;
    public event NoArgEventHandler? WhileButtonHovering;
    public event NoArgEventHandler? OnButtonPressed;

    private event EventHandler<MouseStateEvent>? MouseClickEvent;

    public Button(Label label, Color? panelColor = null, Color? hoverColor = null, Color? disabledColor = null)
    {
        AddChild(Label = label);
        SetColor(panelColor, hoverColor, disabledColor);
        label.PanelComponent.PanelColor = PanelColor.Color;
        CreateClickEvent();

        AddComponent(new AdaptableDebugComponent("Button Data", () =>
        {
            ImGui.Checkbox("Disabled", ref Disabled);
            PanelColor.Color.ImGuiColorEdit("Panel Color");
            HoverColor.Color.ImGuiColorEdit("Hover Color");
            DisabledColor.Color.ImGuiColorEdit("Disabled Color");
        }));
    }

    public Button(string text, Vector2 position, Color? color = null) : this(new Label(text, position), color)
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
        if (Input.CurrentMouseState.IsMouseIn(Label.Rectangle))
        {
            if (!WasHover)
            {
                if (WasDisabled) WasDisabled = false;

                WasHover = true;

                OnButtonHoveringChanged?.Invoke(this, true);
                Label.PanelComponent.PanelColor = HoverColor.Color;
            }

            Input.SetMouseCursor(MOUSE_CURSOR_POINTING_HAND);
        }
        else if (WasDisabled || WasHover)
        {
            if (WasHover)
            {
                WasHover = false;
                OnButtonHoveringChanged?.Invoke(this, false);
            }

            if (WasDisabled) WasDisabled = false;

            Label.PanelComponent.PanelColor = PanelColor.Color;
        }

        if (!Disabled || WasDisabled || Input.MouseOccupier is not null) return;
        WasDisabled = true;
        Label.PanelComponent.PanelColor = DisabledColor.Color;
    }

    public void CreateClickEvent()
    {
        MouseClickEvent = (_, mouseState) =>
        {
            if (Disabled || Input.MouseOccupier is not null)
            {
                if (!mouseState.IsMouseIn(Label.Rectangle)) return;
                Input.SetMouseCursor(MOUSE_CURSOR_NOT_ALLOWED);
                return;
            }

            if (!mouseState.IsMouseIn(Label.Rectangle) || !WasHover || !mouseState[MOUSE_BUTTON_LEFT]) return;
            OnButtonPressed?.Invoke(this);
        };
        Input.MouseEvent += MouseClickEvent;
    }

    public override void RenderLoop()
    {
        Label.Render();
        if (!WasHover) return;
        WhileButtonHovering?.Invoke(this);
    }

    ~Button() => Input.MouseEvent -= MouseClickEvent;
}