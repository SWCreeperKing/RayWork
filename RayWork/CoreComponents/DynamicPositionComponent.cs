using System.Numerics;
using ImGuiNET;

namespace RayWork.CoreComponents;

public class DynamicPositionComponent : TransformComponent
{
    public override Vector2 Position
    {
        get => _position = positionEquation();
        set { }
    }

    public Func<Vector2> positionEquation;

    private Vector2 _position;

    public DynamicPositionComponent(Func<Vector2> positionEquation)
    {
        this.positionEquation = positionEquation;
    }

    public override void Debug()
    {
        ImGui.Text($"Position: {_position}");

        if (!ImGui.Button("Recalculate")) return;
        _position = positionEquation();
    }
}