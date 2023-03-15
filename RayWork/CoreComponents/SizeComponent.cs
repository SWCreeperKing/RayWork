using System.Numerics;
using ImGuiNET;
using RayWork.ECS;

namespace RayWork.CoreComponents;

public class SizeComponent : DebugComponent
{
    public Vector2 size;
    
    public SizeComponent(float width = 0, float height = 0) : this(new Vector2(width, height))
    {
    }

    public SizeComponent(Vector2 size)
    {
        this.size = size;
    }
    
    public void Debug()
    {
        ImGui.DragFloat("Width", ref size.X);
        ImGui.DragFloat("Height", ref size.Y);
    }
}