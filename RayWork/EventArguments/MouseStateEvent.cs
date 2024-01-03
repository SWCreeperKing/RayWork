using System.Numerics;
using Raylib_cs;
using RayWork.CoreComponents;

namespace RayWork.EventArguments;

public class MouseStateEvent : EventArgs
{
    public readonly Vector2 position;
    public readonly MouseButton[] buttonsPressed;
    public readonly MouseButton[] buttonsDown;

    public MouseStateEvent(Vector2 position, IEnumerable<MouseButton> buttonsPressed, IEnumerable<MouseButton> buttonsDown)
    {
        this.position = position;
        this.buttonsPressed = buttonsPressed.ToArray();
        this.buttonsDown = buttonsDown.ToArray();
    }

    public bool this[MouseButton mouseButtonPressed] => buttonsPressed.Contains(mouseButtonPressed);
    public bool IsMouseIn(Rectangle rectangle) => Raylib.CheckCollisionPointRec(position, rectangle);
    public bool IsMouseIn(Objects.Rectangle rectangle) => IsMouseIn(rectangle.RayLibRectangle);
    public bool IsMouseIn(RectangleComponent rectangleComponent) => IsMouseIn(rectangleComponent.RayLibRectangle);
}