using System.Numerics;
using Raylib_cs;
using RayWork.CoreComponents;
using RayWork.CoreComponents.BaseComponents;
using RayWork.Objects;

namespace RayWork.ComponentObjects;

public class Text : GameObject
{
    public TransformComponent TransformComponent;
    public TextComponent TextComponent;

    public Text(string text, TransformComponent transformComponent, Color? color = null)
    {
        AddComponent(TransformComponent = transformComponent);
        AddComponent(TextComponent = new TextComponent(text, color));
    }

    public Text(string text, Vector2 position, Color? color = null) : this(text, new PositionComponent(position), color)
    {
    }

    public override void RenderLoop() => TextComponent.DrawText(TransformComponent.Position, Vector2.Zero);
}