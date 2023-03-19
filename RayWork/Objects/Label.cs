using System.Numerics;
using Raylib_CsLo;
using RayWork.CoreComponents;

namespace RayWork.Objects;

public class Label : GameObject
{
    public PanelComponent panelComponent;
    public TextComponent textComponent;

    public Label(TextComponent textComponent, TransformComponent transformComponent, SizeComponent sizeComponent)
    {
        AddComponent(panelComponent = new PanelComponent(new RectangleComponent(transformComponent, sizeComponent)));
        AddComponent(this.textComponent = textComponent);
    }

    public Label(string text, Vector2 position, Vector2 size) : this(text, (PositionComponent) position,
        (StaticSizeComponent) size)
    {
    }

    public Label(TextComponent textComponent, TransformComponent transformComponent)
    {
        AddComponent(panelComponent = new PanelComponent(
            new RectangleComponent(transformComponent, new DynamicSizeComponent(
                () => this.textComponent.Size()))));
        AddComponent(this.textComponent = textComponent);
    }

    public Label(string text, Vector2 position) : this(text, (PositionComponent) position)
    {
    }
    
    public Label(string text, Rectangle rectangle) : this(text, rectangle.Position(), rectangle.Size())
    {
    }

    public override void RenderLoop()
    {
        panelComponent.DrawPanel();
        textComponent.DrawText(panelComponent.rectangleComponent.Position, Vector2.Zero);
    }
}