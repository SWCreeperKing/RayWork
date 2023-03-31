using System.Numerics;
using RayWork.CoreComponents;
using RayRectangle = Raylib_CsLo.Rectangle;

namespace RayWork.Objects;

public class Label : GameObject
{
    public float Padding
    {
        get => _Padding.X;
        set
        {
            _Padding = new(value);
            SizePadding = _Padding * 2;
        }
    }
    private Vector2 _Padding;

    public Rectangle Rectangle => PanelComponent.RectangleComponent.Rectangle;

    public PanelComponent PanelComponent;
    public TextComponent TextComponent;
    public bool TextPadding;

    private Vector2 SizePadding;

    public Label(TextComponent textComponent, TransformComponent transformComponent, SizeComponent sizeComponent)
    {
        AddComponent(PanelComponent = new(new(transformComponent, sizeComponent)));
        AddComponent(TextComponent = textComponent);
    }

    public Label(string text, Vector2 position, Vector2 size) : this(text, (PositionComponent) position,
        (StaticSizeComponent) size)
    {
    }

    public Label(TextComponent textComponent, TransformComponent transformComponent)
    {
        Padding = 2;
        TextPadding = true;
        AddComponent(TextComponent = textComponent);
        AddComponent(PanelComponent = new(
            new(transformComponent, new DynamicSizeComponent(
                () => TextComponent.Size() + SizePadding))));
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
        var textPosition = PanelComponent.RectangleComponent.Position;
        if (TextPadding) textPosition += _Padding;
        
        PanelComponent.DrawPanel();
        TextComponent.DrawText(textPosition, Vector2.Zero);
    }
}