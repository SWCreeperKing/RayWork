using System.Numerics;
using Raylib_cs;
using RayWork.CoreComponents;
using RayWork.CoreComponents.BaseComponents;
using RayWork.Objects;
using Rectangle = RayWork.Objects.Primitives.Rectangle;

namespace RayWork.ComponentObjects;

public class TextBlock : GameObject
{
    public RectangleComponent RectangleComponent;
    public TextRectangleComponent TextRectangleComponent;

    public TextBlock(string text, TransformComponent position, SizeComponent size, Color? color = null)
    {
        AddComponent(RectangleComponent = new RectangleComponent(position, size));
        AddComponent(TextRectangleComponent = new TextRectangleComponent(text, color));
    }

    public TextBlock(string text, Vector2 position, Vector2 size, Color? color = null) : this(text,
        new PositionComponent(position),
        new StaticSizeComponent(size), color)
    {
    }

    public TextBlock(string text, Rectangle rectangle, Color? color = null) : this(text, rectangle.Position,
        rectangle.Size, color)
    {
    }

    public override void RenderLoop() => TextRectangleComponent.DrawText(RectangleComponent.Rectangle);
}