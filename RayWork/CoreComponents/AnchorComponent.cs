using System.Numerics;
using ImGuiNET;

namespace RayWork.CoreComponents;

public class AnchorComponent : TransformComponent
{
    public Func<Vector2, Vector2> anchorEquation;

    private Vector2 _windowSize;

    public AnchorComponent(Func<Vector2, Vector2> anchorEquation)
    {
        _windowSize = RayApplication.WindowSize;
        this.anchorEquation = anchorEquation;
        position = this.anchorEquation(_windowSize);

        RayApplication.OnWindowSizeChanged += (_, windowChangeArgs) =>
        {
            position = this.anchorEquation(_windowSize = windowChangeArgs.newWindowSize);
        };
    }

    public override void Debug()
    {
        ImGui.Text($"Position: {position}");
        ImGui.Text($"WindowSize: {_windowSize}");
        if (!ImGui.Button("Recalculate")) return;
        position = anchorEquation(_windowSize);
    }
}