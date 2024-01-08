using System.Numerics;
using ImGuiNET;
using RayWork.CoreComponents.BaseComponents;

namespace RayWork.CoreComponents;

public class DynamicSizeComponent : SizeComponent
{
    public override Vector2 Size
    {
        get => _Size = SizeEquation();
        set => Logger.Log("Can not set Size of DynamicSizeComponent");
    }

    private Vector2 _Size;

    public Func<Vector2> SizeEquation;

    public DynamicSizeComponent(Func<Vector2> sizeEquation) => SizeEquation = sizeEquation;

    public override void Debug()
    {
        ImGui.Text($"Size: {_Size}");

        if (!ImGui.Button("Recalculate")) return;
        _Size = SizeEquation();
    }
}