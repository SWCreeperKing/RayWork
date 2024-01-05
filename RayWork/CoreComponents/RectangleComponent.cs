using System.Numerics;
using ImGuiNET;
using RayWork.CoreComponents.BaseComponents;
using RayWork.ECS;
using Rectangle = RayWork.Objects.Primitives.Rectangle;
using RayRectangle = Raylib_cs.Rectangle;

namespace RayWork.CoreComponents;

public class RectangleComponent : DebugComponent
{
    public RayRectangle RayLibRectangle
    {
        get => RectangleHolder.RayLibRectangle;
        set => RectangleHolder.RayLibRectangle = value;
    }

    public Rectangle Rectangle
    {
        get
        {
            var position = PositionHolder.Position;
            var size = SizeHolder.Size;
            if (position != RectangleHolder.Position) RectangleHolder.Position = position;
            if (size != RectangleHolder.Size) RectangleHolder.Size = size;
            return RectangleHolder;
        }
        set
        {
            RectangleHolder.RayLibRectangle = value.RayLibRectangle;
            PositionHolder.Position = value.Position;
            SizeHolder.Size = value.Size;
        }
    }

    public Vector2 Position
    {
        get
        {
            var position = PositionHolder.Position;
            if (position != RectangleHolder.Position) RectangleHolder.Position = position;
            return position;
        }
        set
        {
            PositionHolder.Position = value;
            RectangleHolder.Position = value;
        }
    }

    public Vector2 Size
    {
        get
        {
            var size = SizeHolder.Size;
            if (size != RectangleHolder.Size) RectangleHolder.Size = size;
            return size;
        }
        set
        {
            SizeHolder.Size = value;
            RectangleHolder.Size = value;
        }
    }

    private TransformComponent PositionHolder;
    private SizeComponent SizeHolder;
    private Rectangle RectangleHolder;

    public RectangleComponent(TransformComponent positionHolder, SizeComponent sizeHolder)
    {
        PositionHolder = positionHolder;
        SizeHolder = sizeHolder;
        RectangleHolder = new Rectangle(PositionHolder.Position, SizeHolder.Size);
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

    public override void Debug()
    {
        var pos = PositionHolder.Position;
        var size = SizeHolder.Size;
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

        if (!ImGui.DragFloat("H", ref size.Y)) return;
        Size = size;
    }

    public static implicit operator RectangleComponent(Rectangle rectangle) => new(rectangle);
}