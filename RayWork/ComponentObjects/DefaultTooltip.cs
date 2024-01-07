using System.Numerics;
using ImGuiNET;
using RayWork.CoreComponents;
using RayWork.Objects;

namespace RayWork.ComponentObjects;

public class DefaultTooltip : GameObject
{
    public bool Enabled
    {
        get => TooltipComponent.Enabled;
        set => TooltipComponent.Enabled = value;
    }

    private TooltipComponent TooltipComponent;
    private TextComponent TextComponent;

    public string Text
    {
        get => TextComponent.Text;
        set => TextComponent.Text = value;
    }

    public DefaultTooltip(string text)
    {
        AddComponent(TextComponent = new TextComponent(text));
        AddComponent(TooltipComponent = new TooltipComponent());

        TooltipComponent.GuiTooltipAction = (tc, sq, pos) =>
        {
            var size = TextComponent.Size();
            var correctedPos = tc.CorrectPosition(sq, pos, size);
            tc.DrawGuiBack(correctedPos, size);
            TextComponent.DrawText(correctedPos);
        };
    }

    public override void RenderLoop()
    {
        if (!Enabled) return;
        TooltipComponent.DrawTooltip();
    }
}