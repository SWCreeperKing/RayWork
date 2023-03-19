using System.Numerics;
using ImGuiNET;

namespace RayWork.CoreComponents;

public class PositionComponent : TransformComponent
{
    public override Vector2 Position { get; set; }

    public PositionComponent(float x = 0, float y = 0) : this(new Vector2(x, y))
    {
    }

    public PositionComponent(Vector2 position)
    {
        Position = position;
    }

    public override void Debug()
    {
        var pos = Position;
        if (ImGui.DragFloat("X", ref pos.X) || ImGui.DragFloat("Y", ref pos.Y))
        {
            Position = pos;
        }
    }
}