using System.Numerics;
using ImGuiNET;

namespace RayWork.CoreComponents;

public class PositionComponent : TransformComponent
{
    public PositionComponent(float x = 0, float y = 0) : this(new Vector2(x, y))
    {
    }

    public PositionComponent(Vector2 position)
    {
        this.position = position;
    }

    public override void Debug()
    {
        ImGui.DragFloat("X", ref position.X);
        ImGui.DragFloat("Y", ref position.Y);
    }
}