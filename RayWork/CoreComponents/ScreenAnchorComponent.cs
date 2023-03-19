using System.Numerics;
using ImGuiNET;

namespace RayWork.CoreComponents;

public class ScreenAnchorComponent : TransformComponent
{
    public override Vector2 Position
    {
        get => _position;
        set { }
    }

    public Func<Vector2, Vector2> anchorEquation;

    private Vector2 _position;
    private Vector2 _windowSize;

    public ScreenAnchorComponent(Func<Vector2, Vector2> anchorEquation)
    {
        _windowSize = RayApplication.WindowSize;
        this.anchorEquation = anchorEquation;
        _position = this.anchorEquation(_windowSize);

        RayApplication.OnWindowSizeChanged += (_, windowChangeArgs) =>
        {
            _position = this.anchorEquation(_windowSize = windowChangeArgs.newWindowSize);
        };
    }

    public override void Debug()
    {
        ImGui.Text($"Position: {_position}");
        ImGui.Text($"WindowSize: {_windowSize}");

        if (!ImGui.Button("Recalculate")) return;
        _position = anchorEquation(_windowSize);
    }
}