using System.Numerics;
using ImGuiNET;
using RayWork.ECS;

namespace RayWork.CoreComponents;

public class Transform : DebugComponent
{
    public Vector2 position;
    
    public void Debug()
    {
        ImGui.DragFloat("X", ref position.X);
        ImGui.DragFloat("Y", ref position.Y);
    }
}