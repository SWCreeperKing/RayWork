using System.Numerics;
using ImGuiNET;

namespace RayWork.CoreComponents;

public class AnchorComponent : TransformComponent
{
    public override Vector2 Position
    {
        get => _position = anchorEquation();
        set { }
    }

    public Func<Vector2> anchorEquation;

    private Vector2 _position;

    public AnchorComponent(Func<Vector2> anchorEquation)
    {
        this.anchorEquation = anchorEquation;
    }

    public override void Debug()
    {
        ImGui.Text($"Position: {_position}");

        if (!ImGui.Button("Recalculate")) return;
        _position = anchorEquation();
    }
}