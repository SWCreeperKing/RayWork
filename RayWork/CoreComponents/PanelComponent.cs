using System.Numerics;
using ImGuiNET;
using Raylib_cs;
using RayWork.CoreComponents.BaseComponents;
using RayWork.ECS;
using RayWork.Objects;
using static Raylib_cs.Color;
using Rectangle = RayWork.Objects.Primitives.Rectangle;
using RayRectangle = Raylib_cs.Rectangle;

namespace RayWork.CoreComponents;

public class PanelComponent : DebugComponent
{
    public CompatibleColor PanelColor = new(80, 100, 160, 255);
    public CompatibleColor OutlineColor = BLACK;
    public float OutlineThickness = 2;
    public float Roundness = .2f;
    public int Segments = 10;
    public bool RoundedOutline;
    public bool DrawOutline = true;

    private RectangleComponent RectangleComponent;

    public RayRectangle RayRectangle
    {
        get => RectangleComponent.RayLibRectangle;
        set => RectangleComponent.RayLibRectangle = value;
    }

    public Rectangle Rectangle
    {
        get => RectangleComponent.Rectangle;
        set => RectangleComponent.Rectangle = value;
    }

    public Vector2 Position
    {
        get => RectangleComponent.Position;
        set => RectangleComponent.Position = value;
    }

    public Vector2 Size
    {
        get => RectangleComponent.Size;
        set => RectangleComponent.Size = value;
    }

    public PanelComponent(RectangleComponent rectangleComponent)
    {
        RectangleComponent = rectangleComponent;
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

    public override void Debug()
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