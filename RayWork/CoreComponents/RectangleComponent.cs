using System.Numerics;
using ImGuiNET;
using RayWork.ECS;
using Rectangle = RayWork.Objects.Rectangle;
using RayRectangle = Raylib_cs.Rectangle;

namespace RayWork.CoreComponents;

public class RectangleComponent : IDebugComponent
{
    public RayRectangle RayLibRectangle
    {
        get => _Rectangle.RayLibRectangle;
        set => _Rectangle.RayLibRectangle = value;
    }

    public Rectangle Rectangle
    {
        get
        {
            var position = _Position.Position;
            var size = _Size.Size;
            if (position != _Rectangle.Position) _Rectangle.Position = position;
            if (size != _Rectangle.Size) _Rectangle.Size = size;
            return _Rectangle;
        }
        set
        {
            _Rectangle.RayLibRectangle = value.RayLibRectangle;
            _Position.Position = value.Position;
            _Size.Size = value.Size;
        }
    }

    public Vector2 Position
    {
        get
        {
            var position = _Position.Position;
            if (position != _Rectangle.Position) _Rectangle.Position = position;
            return position;
        }
        set
        {
            _Position.Position = value;
            _Rectangle.Position = value;
        }
    }

    public Vector2 Size
    {
        get
        {
            var size = _Size.Size;
            if (size != _Rectangle.Size) _Rectangle.Size = size;
            return size;
        }
        set
        {
            _Size.Size = value;
            _Rectangle.Size = value;
        }
    }

    private TransformComponent _Position;
    private SizeComponent _Size;
    private Rectangle _Rectangle;

    public RectangleComponent(TransformComponent position, SizeComponent size)
    {
        _Position = position;
        _Size = size;
        _Rectangle = new Rectangle(_Position.Position, _Size.Size);
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
        var pos = _Position.Position;
        var size = _Size.Size;
        if (ImGui.DragFloat("X", ref pos.X))
        {
            Position = pos;
        }

        if (ImGui.DragFloat("Y", ref pos.Y))
        {
            Position = pos;
        }

        if (ImGui.DragFloat("W", ref size.X))
        {
            Size = size;
        }

        if (ImGui.DragFloat("H", ref size.Y))
        {
            Size = size;
        }
    }

    public static implicit operator RectangleComponent(Rectangle rectangle) => new(rectangle);
}