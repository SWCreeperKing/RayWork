using System.Numerics;
using Raylib_CsLo;
using RayWork;
using RayWork.CoreComponents;

namespace RayWorkTester;

public class SimonButton : GameObject
{
    public RectangleComponent rectangleComponent;
    public ButtonComponent buttonComponent;

    public bool active;
    public float activeDurationSeconds = .5f;

    private Color _color;
    private Color _highlightColor;
    private SimonSays _parent;

    public SimonButton(Vector2 position, Vector2 size, Color color)
    {
        AddComponent(rectangleComponent = new RectangleComponent(position, size));
        AddComponent(buttonComponent = new ButtonComponent(rectangleComponent));
        _color = color;
        _highlightColor = _color.MakeLighter();
    }

    public bool ShowOrder()
    {
        if (!active) return false;
        activeDurationSeconds -= RayApplication.DeltaTime;

        if (activeDurationSeconds > 0) return false;
        activeDurationSeconds = .5f;
        active = false;
        return true;
    }

    public override void RenderLoop()
    {
        if (_parent is null) _parent = (SimonSays) Parent;

        if (_parent.readIn && rectangleComponent.Rectangle.IsMouseIn())
        {
            Raylib.DrawRectangleRec(rectangleComponent.Rectangle, _highlightColor);
            buttonComponent.TestClick();
        }
        else Raylib.DrawRectangleRec(rectangleComponent.Rectangle, active ? _highlightColor : _color);
    }

    public void Reset()
    {
        activeDurationSeconds = .5f;
        active = false;
    }
}