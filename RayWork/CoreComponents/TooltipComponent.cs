using System.Numerics;
using ImGuiNET;
using RayWork.EventArguments;
using RayWork.Objects;

namespace RayWork.CoreComponents;

public class TooltipComponent : PanelComponent
{
    public new CompatibleColor PanelColor
    {
        get => base.PanelColor;
        set => base.PanelColor = value;
    }

    public new CompatibleColor OutlineColor
    {
        get => base.OutlineColor;
        set => base.OutlineColor = value;
    }

    public bool Enabled;
    public Action? ImguiTooltipAction;
    public Action<TooltipComponent, ScreenQuadrant, Vector2>? GuiTooltipAction;

    private Vector2 DefaultGrowSize = new(4);
    private Vector2 DefaultGrowSize2 = new(8);

    public TooltipComponent() : base(Vector2.Zero, Vector2.Zero)
    {
        PanelColor = new CompatibleColor(170, 170, 255, 220);
    }

    public TooltipComponent(Action imguiTooltipAction) : base(Vector2.Zero, Vector2.Zero)
        => ImguiTooltipAction = imguiTooltipAction;

    public TooltipComponent(Action<TooltipComponent, ScreenQuadrant, Vector2>? guiTooltipAction) : base(Vector2.Zero,
        Vector2.Zero)
        => GuiTooltipAction = guiTooltipAction;

    public void DrawImguiTooltip()
    {
        if (ImguiTooltipAction is null) return;
        ImGui.BeginTooltip();
        ImguiTooltipAction?.Invoke();
        ImGui.EndTooltip();
    }

    public void DrawGuiTooltip()
    {
        if (GuiTooltipAction is null) return;
        var state = Input.CurrentMouseState;
        GuiTooltipAction(this, state.ScreenQuadrant, state.Position);
    }

    public void DrawTooltip()
    {
        if (ImguiTooltipAction is not null)
        {
            DrawImguiTooltip();
            return;
        }

        if (GuiTooltipAction is null) return;
        DrawGuiTooltip();
    }

    public Vector2 CorrectPosition(ScreenQuadrant screenQuad, Vector2 pos, Vector2 size)
        => new(pos.X - ((int) screenQuad % 2 != 0 ? size.X : 0),
            pos.Y - ((int) screenQuad > 2 ? size.Y : -33));

    public void DrawGuiBack(Vector2 position, Vector2 size, Vector2? grow = null)
    {
        var growRect = grow ?? DefaultGrowSize;
        var growRect2 = grow is null ? DefaultGrowSize2 : grow.Value + grow.Value;
        Position = position - growRect;
        Size = size + growRect2;
        DrawPanel();
    }

    public override void Debug()
    {
        ImGui.Checkbox("Enabled", ref Enabled);
    }
}