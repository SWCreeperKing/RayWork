using System.Numerics;
using ImGuiNET;
using Raylib_CsLo;
using RayWork.ECS;
using RayWork.Objects;

namespace RayWork.CoreComponents;

public class PanelComponent : DebugComponent
{
    public RectangleComponent rectangleComponent;
    public CompatibleColor panelColor;
    public CompatibleColor outlineColor;
    public float outlineThickness = 2;
    public float roundness = .2f;
    public int segments = 10;
    public bool roundedOutline;
    public bool drawOutline = true;

    private Color _panelColor = new(80, 100, 160, 255);
    private Vector4 _panelColorV4;

    private Color _outlineColor = Raylib.BLACK;
    private Vector4 _outlineColorV4;

    public PanelComponent(RectangleComponent rectangleComponent)
    {
        this.rectangleComponent = rectangleComponent;
        panelColor = new Color(80, 100, 160, 255);
        outlineColor = Raylib.BLACK;
    }

    public PanelComponent(TransformComponent transformComponent, SizeComponent sizeComponent) : this(
        new RectangleComponent(transformComponent, sizeComponent))
    {
    }

    public PanelComponent(Vector2 position, Vector2 size) : this(new PositionComponent(position),
        new StaticSizeComponent(size))
    {
    }

    public void DrawPanel()
    {
        if (roundedOutline)
        {
            Raylib.DrawRectangleRounded(rectangleComponent.Rectangle, roundness, segments, panelColor);
            if (!drawOutline) return;
            Raylib.DrawRectangleRoundedLines(rectangleComponent.Rectangle, roundness, segments, outlineThickness,
                outlineColor);
        }
        else
        {
            Raylib.DrawRectangleRec(rectangleComponent.Rectangle, panelColor);
            if (!drawOutline) return;
            Raylib.DrawRectangleLinesEx(rectangleComponent.Rectangle, outlineThickness, outlineColor);
        }
    }

    public void Debug()
    {
        rectangleComponent.Debug();
        panelColor.ImGuiColorEdit("Panel Color");
        outlineColor.ImGuiColorEdit("Outline Color");
        ImGui.DragFloat("Outline Thickness", ref outlineThickness, .5f, 0);
        ImGui.DragFloat("Roundness", ref roundness, .01f, 0, 1);
        ImGui.DragInt("Segments", ref segments, 1, 0);
        ImGui.Checkbox("Round Outline", ref roundedOutline);
        ImGui.Checkbox("Draw Outline", ref drawOutline);
    }
}