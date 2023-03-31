using System.Numerics;
using ImGuiNET;
using Raylib_CsLo;
using RayWork.ECS;
using RayWork.Objects;

namespace RayWork.CoreComponents;

public class PanelComponent : IDebugComponent
{
    public RectangleComponent RectangleComponent;
    public CompatibleColor PanelColor;
    public CompatibleColor OutlineColor;
    public float OutlineThickness = 2;
    public float Roundness = .2f;
    public int Segments = 10;
    public bool RoundedOutline;
    public bool DrawOutline = true;

    public PanelComponent(RectangleComponent rectangleComponent)
    {
        RectangleComponent = rectangleComponent;
        PanelColor = new Color(80, 100, 160, 255);
        OutlineColor = Raylib.BLACK;
    }

    public PanelComponent(TransformComponent transformComponent, SizeComponent sizeComponent) : this(
        new(transformComponent, sizeComponent))
    {
    }

    public PanelComponent(Vector2 position, Vector2 size) : this(new PositionComponent(position),
        new StaticSizeComponent(size))
    {
    }

    public void DrawPanel()
    {
        if (RoundedOutline)
        {
            Raylib.DrawRectangleRounded(RectangleComponent.Rectangle, Roundness, Segments, PanelColor);
            if (!DrawOutline) return;
            Raylib.DrawRectangleRoundedLines(RectangleComponent.Rectangle, Roundness, Segments, OutlineThickness,
                OutlineColor);
        }
        else
        {
            Raylib.DrawRectangleRec(RectangleComponent.Rectangle, PanelColor);
            if (!DrawOutline) return;
            Raylib.DrawRectangleLinesEx(RectangleComponent.Rectangle, OutlineThickness, OutlineColor);
        }
    }

    public void Debug()
    {
        RectangleComponent.Debug();
        PanelColor.ImGuiColorEdit("Panel Color");
        OutlineColor.ImGuiColorEdit("Outline Color");
        ImGui.DragFloat("Outline Thickness", ref OutlineThickness, .5f, 0);
        ImGui.DragFloat("Roundness", ref Roundness, .01f, 0, 1);
        ImGui.DragInt("Segments", ref Segments, 1, 0);
        ImGui.Checkbox("Round Outline", ref RoundedOutline);
        ImGui.Checkbox("Draw Outline", ref DrawOutline);
    }
}