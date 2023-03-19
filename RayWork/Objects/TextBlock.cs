using System.Numerics;
using Raylib_CsLo;
using RayWork.CoreComponents;

namespace RayWork.Objects;

public class TextBlock : GameObject
{
    public RectangleComponent rectangleComponent;
    public TextRectangleComponent textRectangleComponent;
    public ColorComponent colorComponent;

    public TextBlock(string text, TransformComponent position, SizeComponent size, Color? color = null)
    {
        AddComponent(rectangleComponent = new RectangleComponent(position, size));
        AddComponent(textRectangleComponent = new TextRectangleComponent(text));
        AddComponent(colorComponent = new ColorComponent(color ?? Raylib.BLACK));
    }

    public TextBlock(string text, Vector2 position, Vector2 size, Color? color = null) : this(text,
        new PositionComponent(position), new StaticSizeComponent(size), color)
    {
    }

    public TextBlock(string text, Rectangle rectangle, Color? color = null) : this(text, rectangle.Position(),
        rectangle.Size(), color)
    {
    }

    public override void RenderLoop()
    {
        textRectangleComponent.DrawText(colorComponent.Color, rectangleComponent.Rectangle);
    }
}