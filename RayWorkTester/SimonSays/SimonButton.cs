using System.Numerics;
using Raylib_CsLo;
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
    private SimonSays Parent;

    public SimonButton(Vector2 position, Vector2 size, Color color)
    {
        AddComponent(RectangleComponent = new(position, size));
        AddComponent(ButtonComponent = new(RectangleComponent));
        Color = color;
        HighlightColor = Color.MakeLighter();
        Parent = (SimonSays) base.Parent;
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
        if (Parent.ReadIn && RectangleComponent.Rectangle.IsMouseIn())
        {
            Raylib.DrawRectangleRec(RectangleComponent.Rectangle, HighlightColor);
            ButtonComponent.TestClick();
        }
        else Raylib.DrawRectangleRec(RectangleComponent.Rectangle, Active ? HighlightColor : Color);
    }

    public void Reset()
    {
        ActiveDurationSeconds = .5f;
        Active = false;
    }
}