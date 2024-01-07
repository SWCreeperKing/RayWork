using RayWork.CoreComponents;
using RayWork.Objects;

namespace RayWork.ComponentObjects;

public class ImguiTooltip : GameObject
{
    public bool Enabled
    {
        get => TooltipComponent.Enabled;
        set => TooltipComponent.Enabled = value;
    }

    private TooltipComponent TooltipComponent;

    public ImguiTooltip(Action action) => AddComponent(TooltipComponent = new TooltipComponent(action));

    public override void RenderLoop()
    {
        if (!Enabled) return;
        TooltipComponent.DrawImguiTooltip();
    }
}