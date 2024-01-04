using System.Numerics;
using ImGuiNET;
using RayWork.CoreComponents.BaseComponents;

namespace RayWork.CoreComponents;

public class ScreenAnchorComponent : TransformComponent
{
    public override Vector2 Position
    {
        get => PositionHolder;
        set => Logger.Log("Can not set Position of ScreenAnchorComponent");
    }
    private Vector2 PositionHolder;

    public Func<Vector2, Vector2> AnchorEquation;

    private Vector2 WindowSize;

    public ScreenAnchorComponent(Func<Vector2, Vector2> anchorEquation)
    {
        WindowSize = RayApplication.WindowSize;
        AnchorEquation = anchorEquation;
        PositionHolder = AnchorEquation(WindowSize);

        RayApplication.OnWindowSizeChanged += (_, windowChangeArgs) =>
        {
            PositionHolder = AnchorEquation(WindowSize = windowChangeArgs.NewWindowSize);
        };
    }

    public override void Debug()
    {
        ImGui.Text($"Position: {PositionHolder}");
        ImGui.Text($"WindowSize: {WindowSize}");

        if (!ImGui.Button("Recalculate")) return;
        PositionHolder = AnchorEquation(WindowSize);
    }
}