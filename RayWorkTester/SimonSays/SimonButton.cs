using System.Numerics;
using ImGuiNET;
using Raylib_cs;
using RayWork;
using RayWork.CoreComponents;
using RayWork.Objects;
using RayWork.Objects.Primitives;

namespace RayWorkTester.SimonSays;

public class SimonButton : GameObject
{
    private readonly RectangleComponent RectangleComponent;
    private readonly ButtonComponent ButtonComponent;

    public bool Active;
    public float ActiveDurationSeconds = .5f;

    private CompatibleColor Color;
    private CompatibleColor HighlightColor;
    private new SimonSays? Parent;

    public event NoArgEventHandler? OnClicked
    {
        add => ButtonComponent.OnClicked += value;
        remove => ButtonComponent.OnClicked -= value;
    }

    public SimonButton(Vector2 position, Vector2 size, Color color)
    {
        Color = color;
        HighlightColor = Color.MakeLighter();

        AddComponent(RectangleComponent = new RectangleComponent(position, size));
        AddComponent(ButtonComponent = new ButtonComponent(RectangleComponent));

        AddComponent(new AdaptableDebugComponent("Simon Button Data", () =>
        {
            ImGui.DragFloat("Active Duration Seconds", ref ActiveDurationSeconds);
            Color.ImGuiColorEdit("Color");
            HighlightColor.ImGuiColorEdit("Highlight Color");
        }));
    }

    public bool ShowOrder()
    {
        if (!Active) return false;
        ActiveDurationSeconds -= RayApplication.DeltaTime;

        if (ActiveDurationSeconds > 0) return false;
        ActiveDurationSeconds = .5f;
        Active = false;
        return true;
    }

    public override void RenderLoop()
    {
        Parent ??= (SimonSays) base.Parent!;

        if (Parent.ReadIn && RectangleComponent.Rectangle.IsMouseIn())
        {
            Raylib.DrawRectangleRec(RectangleComponent.Rectangle, HighlightColor);
        }
        else Raylib.DrawRectangleRec(RectangleComponent.Rectangle, Active ? HighlightColor : Color);
    }

    public void Reset()
    {
        ActiveDurationSeconds = .5f;
        Active = false;
    }
}