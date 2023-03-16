using System.Numerics;
using Raylib_CsLo;
using RayWork.CoreComponents;

namespace RayWork.Objects;

public class TextBlock : GameObject
{
    public RectangleComponent rectangleComponent;
    public TextRectangleComponent textRectangleComponent;
    public ColorComponent colorComponent;

    public TextBlock(string text, Rectangle rectangle, Color color)
    {
        AddComponent(rectangleComponent = new RectangleComponent(rectangle));
        AddComponent(textRectangleComponent = new TextRectangleComponent(text));
        AddComponent(colorComponent = new ColorComponent(color));
    }

    public TextBlock(string text, Vector2 position, Vector2 size, Color color) : this(text, position.Rect(size), color)
    {
    }

    public override void RenderLoop()
    {
        textRectangleComponent.DrawText(colorComponent.color, rectangleComponent.Rectangle);
    }
}