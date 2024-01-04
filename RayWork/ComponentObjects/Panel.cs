using System.Numerics;
using RayWork.CoreComponents;
using RayWork.Objects;
using RayWork.Objects.Primitives;
using RayRectangle = Raylib_cs.Rectangle;

namespace RayWork.ComponentObjects;

public class Panel : GameObject
{
    public PanelComponent PanelComponent;

    public Panel(Vector2 position, Vector2 size) => AddComponent(PanelComponent = new PanelComponent(position, size));

    public Panel(Rectangle rectangle) : this(rectangle.Position, rectangle.Size)
    {
    }

    public Panel(RayRectangle rectangle) : this(rectangle.Position(), rectangle.Size())
    {
    }

    public override void RenderLoop() => PanelComponent.DrawPanel();
}