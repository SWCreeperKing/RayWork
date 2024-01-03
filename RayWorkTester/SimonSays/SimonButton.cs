using System.Numerics;
using Raylib_cs;
using RayWork;
using RayWork.CoreComponents;

namespace RayWorkTester;

public class SimonButton : GameObject
{
    public RectangleComponent RectangleComponent;
    public ButtonComponent ButtonComponent;

    public bool Active;
    public float ActiveDurationSeconds = .5f;

    private Color Color;
    private Color HighlightColor;
    private SimonSays? Parent;

    public SimonButton(Vector2 position, Vector2 size, Color color)
    {
        AddComponent(RectangleComponent = new RectangleComponent(position, size));
        AddComponent(ButtonComponent = new ButtonComponent(RectangleComponent));
        Color = color;
        HighlightColor = Color.MakeLighter();
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
        Parent ??= (SimonSays) base.Parent;

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