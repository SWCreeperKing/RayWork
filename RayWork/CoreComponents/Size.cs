using System.Numerics;
using ImGuiNET;
using RayWork.ECS;

namespace RayWork.CoreComponents;

public class Size : DebugComponent
{
    public Vector2 size;
    
    public void Debug()
    {
        ImGui.DragFloat("Width", ref size.X);
        ImGui.DragFloat("Height", ref size.Y);
    }
}