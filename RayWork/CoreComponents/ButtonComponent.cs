using System.Numerics;
using ImGuiNET;
using RayWork.ECS;
using static Raylib_CsLo.MouseButton;
using static Raylib_CsLo.Raylib;

namespace RayWork.CoreComponents;

public class ButtonComponent : DebugComponent
{
    public RectangleComponent rectangleComponent;

    public ButtonComponent(RectangleComponent rectangleComponent)
    {
        this.rectangleComponent = rectangleComponent;
    }

    public ButtonComponent(TransformComponent transformComponent, SizeComponent sizeComponent) : this(
        new RectangleComponent(transformComponent, sizeComponent))
    {
    }

    public ButtonComponent(Vector2 position, Vector2 size) : this((PositionComponent) position,
        (StaticSizeComponent) size)
    {
    }

    public EventHandler OnClicked;

    public bool TestClick()
    {
        if (!rectangleComponent.Rectangle.IsMouseIn() || !IsMouseButtonPressed(MOUSE_BUTTON_LEFT)) return false;
        if (OnClicked is not null) OnClicked(null, null);
        return true;
    }

    public void Debug()
    {
        ImGui.Text(OnClicked is null ? "No Events assigned" : "Events are active");
    }
}