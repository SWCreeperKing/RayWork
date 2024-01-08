using System.Numerics;
using ImGuiNET;
using Raylib_cs;
using RayWork.CoreComponents;
using RayWork.Objects;
using Rectangle = RayWork.Objects.Primitives.Rectangle;

namespace RayWork.ComponentObjects.Togglables;

public abstract class ToggleButton : GameObject
{
    public bool IsToggled;

    private ButtonComponent ButtonComponent;
    private bool IsHovering;

    public ToggleButton(Vector2 pos, Vector2 size) => AddComponent(ButtonComponent = new ButtonComponent(pos, size));
    public ToggleButton(Rectangle rect) => AddComponent(ButtonComponent = new ButtonComponent(rect));

    public override void UpdateLoop()
    {
        IsHovering = Input.CurrentMouseState.IsMouseIn(ButtonComponent.Rectangle);
        if (!IsHovering || !Input.CurrentMouseState[MouseButton.MOUSE_BUTTON_LEFT]) return;
        IsToggled = !IsToggled;
    }

    public override void RenderLoop()
    {
        if (IsToggled)
        {
            DrawToggledOn(IsHovering);
        }
        else
        {
            DrawToggledOff(IsHovering);
        }
    }

    public override void DebugLoop() => ImGui.Checkbox("Toggled", ref IsToggled);

    public abstract void DrawToggledOn(bool isHovering);
    public abstract void DrawToggledOff(bool isHovering);
}