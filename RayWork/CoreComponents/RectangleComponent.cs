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
        get => _position.Position;
        set
        {
            _position.Position = value;
            RecalcRectangle();
        }
    }

    public Vector2 Size
    {
        get => _size.Size;
        set
        {
            _size.Size = value;
            RecalcRectangle();
        }
    }

    private TransformComponent _position;
    private SizeComponent _size;
    private Rectangle _rectangle;

    public RectangleComponent(TransformComponent position, SizeComponent size)
    {
        _position = position;
        _size = size;
        RecalcRectangle();
    }

    public RectangleComponent(Vector2 position, Vector2 size) : this((PositionComponent) position,
        (StaticSizeComponent) size)
    {
    }

    public RectangleComponent(Rectangle rectangle) : this(rectangle.Position(), rectangle.Size())
    {
    }

    public void RecalcRectangle()
    {
        _rectangle = _position.Position.Rect(_size.Size);
    }

    public void Debug()
    {
        var pos = _position.Position;
        var size = _size.Size;
        if (ImGui.DragFloat("X", ref pos.X)) UpdatePosition(pos);
        if (ImGui.DragFloat("Y", ref pos.Y)) UpdatePosition(pos);
        if (ImGui.DragFloat("W", ref size.X)) UpdateSize(size);
        if (ImGui.DragFloat("H", ref size.Y)) UpdateSize(size);
    }

    public void UpdatePosition(Vector2 position)
    {
        _position.Position = position;
        RecalcRectangle();
    }

    public void UpdateSize(Vector2 size)
    {
        _size.Size = size;
        RecalcRectangle();
    }

    public static implicit operator RectangleComponent(Rectangle rectangle) => new(rectangle);
}