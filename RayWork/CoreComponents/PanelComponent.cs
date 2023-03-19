using System.Numerics;
using ImGuiNET;
using Raylib_CsLo;
using RayWork.ECS;
using RayWork.RLImgui;

namespace RayWork.CoreComponents;

public class PanelComponent : DebugComponent
{
    public Color PanelColor
    {
        get => _panelColor;
        set
        {
            _panelColor = value;
            _panelColorV4 = _panelColor.ToV4();
        }
    }

    public Color OutlineColor
    {
        get => _outlineColor;
        set
        {
            _outlineColor = value;
            _outlineColorV4 = _outlineColor.ToV4();
        }
    }

    public RectangleComponent rectangleComponent;
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
        _panelColorV4 = _panelColor.ToV4();
        _outlineColorV4 = _outlineColor.ToV4();
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
            Raylib.DrawRectangleRounded(rectangleComponent.Rectangle, roundness, segments, PanelColor);
            if (!drawOutline) return;
            Raylib.DrawRectangleRoundedLines(rectangleComponent.Rectangle, roundness, segments, outlineThickness,
                OutlineColor);
        }
        else
        {
            Raylib.DrawRectangleRec(rectangleComponent.Rectangle, PanelColor);
            if (!drawOutline) return;
            Raylib.DrawRectangleLinesEx(rectangleComponent.Rectangle, outlineThickness, OutlineColor);
        }
    }

    public void Debug()
    {
        rectangleComponent.Debug();
        if (ImGui.ColorEdit4("Panel Color", ref _panelColorV4)) PanelColor = _panelColorV4.ToColor();
        if (ImGui.ColorEdit4("Outline Color", ref _outlineColorV4)) OutlineColor = _outlineColorV4.ToColor();
        ImGui.DragFloat("Outline Thickness", ref outlineThickness, .5f, 0);
        ImGui.DragFloat("Roundness", ref roundness, .01f, 0, 1);
        ImGui.DragInt("Segments", ref segments, 1, 0);
        ImGui.Checkbox("Round Outline", ref roundedOutline);
        ImGui.Checkbox("Draw Outline", ref drawOutline);
    }
}