using System.Numerics;
using ImGuiNET;
using RayWork.CoreComponents.BaseComponents;

namespace RayWork.CoreComponents;

public class PositionComponent : TransformComponent
{
    public override Vector2 Position { get; set; }

    public PositionComponent(float x = 0, float y = 0) : this(new Vector2(x, y))
    {
    }

    public PositionComponent(Vector2 position) => Position = position;

    public override void Debug()
    {
        var pos = Position;
        if (ImGui.DragFloat("X", ref pos.X)) Position = pos;

        if (!ImGui.DragFloat("Y", ref pos.Y)) return;
        Position = pos;
    }

    public static implicit operator PositionComponent(Vector2 position) => new(position);
}