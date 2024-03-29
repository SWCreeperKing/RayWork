using System.Numerics;
using ImGuiNET;
using RayWork.CoreComponents.BaseComponents;

namespace RayWork.CoreComponents;

public class StaticSizeComponent : SizeComponent
{
    public override Vector2 Size { get; set; }

    public StaticSizeComponent(float width = 0, float height = 0) : this(new Vector2(width, height))
    {
    }

    public StaticSizeComponent(Vector2 size) => Size = size;

    public override void Debug()
    {
        var size = Size;
        if (ImGui.DragFloat("Width", ref size.X))
        {
            Size = size;
        }

        if (!ImGui.DragFloat("Height", ref size.Y)) return;
        Size = size;
    }

    public static implicit operator StaticSizeComponent(Vector2 size) => new(size);
}