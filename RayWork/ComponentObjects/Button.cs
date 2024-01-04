using System.Numerics;
using ImGuiNET;
using Raylib_cs;
using RayWork.CoreComponents;
using RayWork.EventArguments;
using RayWork.Objects;
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

    public EventHandler? OnButtonHover;
    public EventHandler? OnButtonPressed;

    private EventHandler<MouseStateEvent>? MouseClickEvent;

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
        if (!Disabled || WasDisabled || Input.MouseOccupier is not null) return;
        if (WasHover)
        {
            WasHover = false;
        }

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

            if (mouseState.IsMouseIn(Label.Rectangle))
            {
                if (!WasHover)
                {
                    if (WasDisabled)
                    {
                        WasDisabled = false;
                    }

                    WasHover = true;

                    OnButtonHover?.Invoke(null, null);
                    Label.PanelComponent.PanelColor = HoverColor.Color;
                }

                Input.SetMouseCursor(MOUSE_CURSOR_POINTING_HAND);
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

            if (!mouseState.IsMouseIn(Label.Rectangle) || !WasHover || !mouseState[MOUSE_BUTTON_LEFT] ||
                OnButtonPressed is null) return;
            OnButtonPressed(null, null!);
        };
        Input.MouseEvent += MouseClickEvent;
    }

    public override void RenderLoop() => Label.Render();

    ~Button() => Input.MouseEvent -= MouseClickEvent;
}