using System.Numerics;
using Raylib_cs;
using RayWork.CoreComponents;

namespace RayWork.EventArguments;

public class MouseStateEvent(MouseState state)
    : EventArgs
{
    public readonly Vector2 Position = state.Position;
    public readonly ScreenQuadrant ScreenQuadrant = state.ScreenQuadrant;
    public readonly IReadOnlyCollection<MouseButton> ButtonsPressed = state.ButtonsPressed;
    public readonly IReadOnlyCollection<MouseButton> ButtonsDown = state.ButtonsDown;

    public bool this[MouseButton mouseButtonPressed] => ButtonsPressed.Contains(mouseButtonPressed);
    public bool IsMouseIn(Rectangle rectangle) => Raylib.CheckCollisionPointRec(Position, rectangle);
    public bool IsMouseIn(Objects.Primitives.Rectangle rectangle) => IsMouseIn(rectangle.RayLibRectangle);
    public bool IsMouseIn(RectangleComponent rectangleComponent) => IsMouseIn(rectangleComponent.RayLibRectangle);
}

public readonly struct MouseState(
    Vector2 position,
    ScreenQuadrant screenQuadrant,
    IEnumerable<MouseButton> buttonsPressed,
    IEnumerable<MouseButton> buttonsDown)
{
    public readonly Vector2 Position = position;
    public readonly ScreenQuadrant ScreenQuadrant = screenQuadrant; 
    public readonly IReadOnlyCollection<MouseButton> ButtonsPressed = buttonsPressed.ToArray();
    public readonly IReadOnlyCollection<MouseButton> ButtonsDown = buttonsDown.ToArray();

    public bool this[MouseButton mouseButtonPressed] => ButtonsPressed.Contains(mouseButtonPressed);
    public bool IsMouseIn(Rectangle rectangle) => Raylib.CheckCollisionPointRec(Position, rectangle);
    public bool IsMouseIn(Objects.Primitives.Rectangle rectangle) => IsMouseIn(rectangle.RayLibRectangle);
    public bool IsMouseIn(RectangleComponent rectangleComponent) => IsMouseIn(rectangleComponent.RayLibRectangle);
}

public enum ScreenQuadrant
{
    TopLeft = 1,
    TopRight = 2,
    BottomLeft = 3,
    BottomRight = 4
}