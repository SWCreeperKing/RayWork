using System.Numerics;
using ImGuiNET;
using RayWork.CoreComponents.BaseComponents;
using RayWork.ECS;
using RayWork.EventArguments;
using static Raylib_cs.MouseButton;

namespace RayWork.CoreComponents;

public class ButtonComponent : DebugComponent
{
    public RectangleComponent RectangleComponent;
    public EventHandler? OnClicked;

    private EventHandler<MouseStateEvent>? MouseClickEvent;

    public ButtonComponent(RectangleComponent rectangleComponent)
    {
        RectangleComponent = rectangleComponent;

        MouseClickEvent = (_, mouseEvent) =>
        {
            if (!mouseEvent.IsMouseIn(rectangleComponent) || OnClicked is null ||
                !mouseEvent[MOUSE_BUTTON_LEFT]) return;
            OnClicked(null, null!);
        };
        Input.MouseEvent += MouseClickEvent;
    }

    public ButtonComponent(TransformComponent transformComponent, SizeComponent sizeComponent) : this(
        new RectangleComponent(transformComponent, sizeComponent))
    {
    }

    public ButtonComponent(Vector2 position, Vector2 size) : this((PositionComponent) position,
        (StaticSizeComponent) size)
    {
    }

    public override void Debug() => ImGui.Text(OnClicked is null ? "No Events assigned" : "Events are active");

    ~ButtonComponent() => Input.MouseEvent -= MouseClickEvent;
}