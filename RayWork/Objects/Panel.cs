using System.Numerics;
using Raylib_CsLo;
using RayWork.CoreComponents;

namespace RayWork.Objects;

public class Panel : GameObject
{
    public PanelComponent panelComponent;

    public Panel(Vector2 position, Vector2 size)
    {
        AddComponent(panelComponent = new PanelComponent(position, size));
    }

    public Panel(Rectangle rectangle) : this(rectangle.Position(), rectangle.Size())
    {
    }

    public override void RenderLoop()
    {
        panelComponent.DrawPanel();
    }
}