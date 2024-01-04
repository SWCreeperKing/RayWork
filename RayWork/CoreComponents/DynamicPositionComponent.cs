using System.Numerics;
using ImGuiNET;
using RayWork.CoreComponents.BaseComponents;

namespace RayWork.CoreComponents;

public class DynamicPositionComponent(Func<Vector2> positionEquation) : TransformComponent
{
    public override Vector2 Position
    {
        get => PositionHolder = PositionEquation();
        set => Logger.Log("Can not set Position of DynamicPositionComponent");
    }

    private Vector2 PositionHolder;

    public Func<Vector2> PositionEquation = positionEquation;

    public override void Debug()
    {
        ImGui.Text($"Position: {PositionHolder}");

        if (!ImGui.Button("Recalculate")) return;
        PositionHolder = PositionEquation();
    }
}