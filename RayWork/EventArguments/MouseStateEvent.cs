using System.Numerics;
using Raylib_cs;
using RayWork.CoreComponents;

namespace RayWork.EventArguments;

public class MouseStateEvent(
    Vector2 position,
    IEnumerable<MouseButton> buttonsPressed,
    IEnumerable<MouseButton> buttonsDown)
    : EventArgs
{
    public readonly Vector2 Position = position;
    public readonly MouseButton[] ButtonsPressed = buttonsPressed.ToArray();
    public readonly MouseButton[] ButtonsDown = buttonsDown.ToArray();

    public bool this[MouseButton mouseButtonPressed] => ButtonsPressed.Contains(mouseButtonPressed);
    public bool IsMouseIn(Rectangle rectangle) => Raylib.CheckCollisionPointRec(Position, rectangle);
    public bool IsMouseIn(Objects.Primitives.Rectangle rectangle) => IsMouseIn(rectangle.RayLibRectangle);
    public bool IsMouseIn(RectangleComponent rectangleComponent) => IsMouseIn(rectangleComponent.RayLibRectangle);
}