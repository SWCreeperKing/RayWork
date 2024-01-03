using Raylib_CsLo;

namespace RayWork.Objects;

public class Scrollbar : GameObject
{
    public Panel Slider;
    public Panel Bar;

    public Scrollbar()
    {
        // AddChild(Slider = new Panel());
    }

    public override void UpdateLoop()
    {
    }

    public override void RenderLoop()
    {
    }
}