using System.Numerics;
using ImGuiNET;
using Raylib_CsLo;
using RayWork.ECS;

namespace RayWork.CoreComponents;

public class RectangleComponent : DebugComponent
{
    public Rectangle Rectangle => _rectangle;

    public Vector2 Position
    {
        get => _position;
        set
        {
            _position = value;
            RecalcRectangle();
        }
    }

    public Vector2 Size
    {
        get => _size;
        set
        {
            _size = value;
            RecalcRectangle();
        }
    }

    private Vector2 _position;
    private Vector2 _size;
    private Rectangle _rectangle;

    public RectangleComponent(Vector2 position, Vector2 size)
    {
        _position = position;
        _size = size;
        RecalcRectangle();
    }

    public RectangleComponent(Rectangle rectangle) : this(rectangle.Position(), rectangle.Position())
    {
    }

    public void RecalcRectangle()
    {
        _rectangle = _position.Rect(_size);
    }

    public void Debug()
    {
        if (ImGui.DragFloat("X", ref _position.X)) RecalcRectangle();
        if (ImGui.DragFloat("Y", ref _position.Y)) RecalcRectangle();
        if (ImGui.DragFloat("W", ref _size.X)) RecalcRectangle();
        if (ImGui.DragFloat("H", ref _size.Y)) RecalcRectangle();
    }
}