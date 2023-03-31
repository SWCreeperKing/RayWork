using System.Numerics;
using RayWork.CoreComponents;
using RayRectangle = Raylib_CsLo.Rectangle;

namespace RayWork.Objects;

public class Label : GameObject
{
    public float Padding
    {
        get => _padding.X;
        set
        {
            _padding = new Vector2(value);
            _sizePadding = _padding * 2;
        }
    }

    public Rectangle rectangle => panelComponent.rectangleComponent.Rectangle;

    public PanelComponent panelComponent;
    public TextComponent textComponent;
    public bool textPadding;

    private Vector2 _padding;
    private Vector2 _sizePadding;

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
        Padding = 2;
        textPadding = true;
        AddComponent(this.textComponent = textComponent);
        AddComponent(panelComponent = new PanelComponent(
            new RectangleComponent(transformComponent, new DynamicSizeComponent(
                () => this.textComponent.Size() + _sizePadding))));
    }

    public Label(string text, Vector2 position) : this(text, (PositionComponent) position)
    {
    }

    public Label(string text, Rectangle rectangle) : this(text, rectangle.Position, rectangle.Size)
    {
    }
    public Label(string text, RayRectangle rectangle) : this(text, rectangle.Position(), rectangle.Size())
    {
    }

    public override void RenderLoop()
    {
        var textPosition = panelComponent.rectangleComponent.Position;
        if (textPadding) textPosition += _padding;
        
        panelComponent.DrawPanel();
        textComponent.DrawText(textPosition, Vector2.Zero);
    }
}