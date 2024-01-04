using System.Numerics;
using RayWork.CoreComponents;
using RayWork.CoreComponents.BaseComponents;
using RayWork.Objects;
using RayWork.Objects.Primitives;
using RayRectangle = Raylib_cs.Rectangle;

namespace RayWork.ComponentObjects;

public class Label : GameObject
{
    public float Padding
    {
        get => PaddingHolder.X;
        set
        {
            PaddingHolder = new Vector2(value);
            SizePadding = PaddingHolder * 2;
        }
    }

    private Vector2 PaddingHolder;

    public Rectangle Rectangle => PanelComponent.Rectangle;

    public PanelComponent PanelComponent;
    public TextComponent TextComponent;
    public bool TextPadding;

    private Vector2 SizePadding;

    public Label(TextComponent textComponent, TransformComponent transformComponent, SizeComponent sizeComponent)
    {
        AddComponent(PanelComponent = new PanelComponent(new RectangleComponent(transformComponent, sizeComponent)));
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
        AddComponent(PanelComponent = new PanelComponent(
            new RectangleComponent(transformComponent, new DynamicSizeComponent(
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
        var textPosition = PanelComponent.Position;
        if (TextPadding) textPosition += PaddingHolder;

        PanelComponent.DrawPanel();
        TextComponent.DrawText(textPosition, Vector2.Zero);
    }
}