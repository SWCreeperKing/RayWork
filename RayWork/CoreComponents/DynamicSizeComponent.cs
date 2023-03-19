using System.Numerics;
using ImGuiNET;

namespace RayWork.CoreComponents;

public class DynamicSizeComponent : SizeComponent
{
    public override Vector2 Size
    {
        get => _size = sizeEquation();
        set { }
    }

    public Func<Vector2> sizeEquation;

    private Vector2 _size;

    public DynamicSizeComponent(Func<Vector2> sizeEquation)
    {
        this.sizeEquation = sizeEquation;
    }

    public override void Debug()
    {
        ImGui.Text($"Size: {_size}");

        if (!ImGui.Button("Recalculate")) return;
        _size = sizeEquation();
    }
}