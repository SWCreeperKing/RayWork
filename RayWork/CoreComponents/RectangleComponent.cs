using System.Numerics;
using ImGuiNET;
using RayWork.ECS;
using Rectangle = RayWork.Objects.Rectangle;
using RayRectangle = Raylib_CsLo.Rectangle;

namespace RayWork.CoreComponents;

public class RectangleComponent : DebugComponent
{
    public RayRectangle RayLibRectangle
    {
        get => _rectangle.RayLibRectangle;
        set => _rectangle.RayLibRectangle = value;
    }
    
    public Rectangle Rectangle
    {
        get
        {
            var position = _position.Position;
            var size = _size.Size;
            if (position != _rectangle.Position) _rectangle.Position = position;
            if (size != _rectangle.Size) _rectangle.Size = size;
            return _rectangle;
        }
        set
        {
            _rectangle.RayLibRectangle = value.RayLibRectangle;
            _position.Position = value.Position;
            _size.Size = value.Size;
        }
    }

    public Vector2 Position
    {
        get
        {
            var position = _position.Position;
            if (position != _rectangle.Position) _rectangle.Position = position;
            return position;
        }
        set
        {
            _position.Position = value;
            _rectangle.Position = value;
        }
    }

    public Vector2 Size
    {
        get
        {
            var size = _size.Size;
            if (size != _rectangle.Size) _rectangle.Size = size;
            return size;
        }
        set
        {
            _size.Size = value;
            _rectangle.Size = value;
        }
    }

    private TransformComponent _position;
    private SizeComponent _size;
    private Rectangle _rectangle;

    public RectangleComponent(TransformComponent position, SizeComponent size)
    {
        _position = position;
        _size = size;
        _rectangle = new(_position.Position, _size.Size);
    }

    public RectangleComponent(Vector2 position, Vector2 size) : this((PositionComponent) position,
        (StaticSizeComponent) size)
    {
    }

    public RectangleComponent(Rectangle rectangle) : this(rectangle.Position, rectangle.Size)
    {
    }

    public RectangleComponent(RayRectangle rectangle) : this(rectangle.Position(), rectangle.Size())
    {
    }

    public void Debug()
    {
        var pos = _position.Position;
        var size = _size.Size;
        if (ImGui.DragFloat("X", ref pos.X)) Position = pos;
        if (ImGui.DragFloat("Y", ref pos.Y)) Position = pos;
        if (ImGui.DragFloat("W", ref size.X)) Size = size;
        if (ImGui.DragFloat("H", ref size.Y)) Size = size;
    }

    public static implicit operator RectangleComponent(Rectangle rectangle) => new(rectangle);
}