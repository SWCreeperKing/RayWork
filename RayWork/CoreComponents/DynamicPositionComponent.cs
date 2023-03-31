using System.Numerics;
using ImGuiNET;

namespace RayWork.CoreComponents;

public class DynamicPositionComponent : TransformComponent
{
    public override Vector2 Position
    {
        get => _Position = PositionEquation();
        set { }
    }

    private Vector2 _Position;

    public Func<Vector2> PositionEquation;

    public DynamicPositionComponent(Func<Vector2> positionEquation) => PositionEquation = positionEquation;

    public override void Debug()
    {
        ImGui.Text($"Position: {_Position}");

        if (!ImGui.Button("Recalculate")) return;
        _Position = PositionEquation();
    }
}