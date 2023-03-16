using System.Numerics;
using Raylib_CsLo;
using RayWork.CoreComponents;

namespace RayWork.Objects;

public class Text : GameObject
{
    public PositionComponent positionComponent;
    public ColorComponent colorComponent;
    public TextComponent textComponent;
    
    public Text(string text, Vector2 position)
    {
        AddComponent(positionComponent = new PositionComponent(position));
        AddComponent(colorComponent = new ColorComponent(Raylib.BLACK));
        AddComponent(textComponent = new TextComponent(text));
    }

    public override void RenderLoop()
    {
        textComponent.DrawText(positionComponent.position, Vector2.Zero, colorComponent.color);
    }
}