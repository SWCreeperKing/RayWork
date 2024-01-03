using System.Numerics;
using ImGuiNET;
using RayWork.ECS;
using RayWork.EventArguments;
using static Raylib_CsLo.MouseButton;
using static Raylib_CsLo.Raylib;

namespace RayWork.CoreComponents;

public class ButtonComponent : IDebugComponent
{
    public RectangleComponent RectangleComponent;

    private EventHandler<MouseStateEvent> MouseClickEvent;

    public ButtonComponent(RectangleComponent rectangleComponent)
    {
        RectangleComponent = rectangleComponent;

        MouseClickEvent = (_, mouseEvent) =>
        {
            if (!mouseEvent.IsMouseIn(rectangleComponent) || OnClicked is null || !mouseEvent[MOUSE_BUTTON_LEFT]) return;
            OnClicked(null, null);
        };
        Input.MouseEvent += MouseClickEvent;
    }

    public ButtonComponent(TransformComponent transformComponent, SizeComponent sizeComponent) : this(
        new(transformComponent, sizeComponent))
    {
    }

    public ButtonComponent(Vector2 position, Vector2 size) : this((PositionComponent) position,
        (StaticSizeComponent) size)
    {
    }

    public EventHandler OnClicked;

    public void Debug() => ImGui.Text(OnClicked is null ? "No Events assigned" : "Events are active");

    ~ButtonComponent()
    {
        Input.MouseEvent -= MouseClickEvent;
    }
}