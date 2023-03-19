using System.Numerics;
using Raylib_CsLo;
using RayWork.CoreComponents;

namespace RayWork.Objects;

public class Text : GameObject
{
    public TransformComponent transformComponent;
    public ColorComponent colorComponent;
    public TextComponent textComponent;

    public Text(string text, TransformComponent transformComponent, Color? color = null)
    {
        AddComponent(this.transformComponent = transformComponent);
        AddComponent(colorComponent = new ColorComponent(color ?? Raylib.BLACK));
        AddComponent(textComponent = new TextComponent(text));
    }

    public Text(string text, Vector2 position, Color? color = null) : this(text, new PositionComponent(position), color)
    {
    }

    public override void RenderLoop()
    {
        textComponent.DrawText(transformComponent.Position, Vector2.Zero, colorComponent.Color);
    }
}