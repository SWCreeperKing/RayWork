using System.Numerics;
using Raylib_CsLo;
using RayWork.CoreComponents;

namespace RayWork.EventArguments;

public class MouseStateEvent : EventArgs
{
    public readonly Vector2 position;
    public readonly MouseButton[] buttonsPressed;
    public readonly MouseButton[] buttonsDown;

    public MouseStateEvent(Vector2 position, MouseButton[] buttonsPressed, MouseButton[] buttonsDown)
    {
        this.position = position;
        this.buttonsPressed = buttonsPressed;
        this.buttonsDown = buttonsDown;
    }

    public bool this[MouseButton mouseButtonPressed]
    {
        get => buttonsPressed.Contains(mouseButtonPressed);
    }

    public bool IsMouseIn(Rectangle rectangle) => Raylib.CheckCollisionPointRec(position, rectangle);
    public bool IsMouseIn(Objects.Rectangle rectangle) => IsMouseIn(rectangle.RayLibRectangle);
    public bool IsMouseIn(RectangleComponent rectangleComponent) => IsMouseIn(rectangleComponent.RayLibRectangle);
}