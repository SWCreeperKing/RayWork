using RayWork.ECS;

namespace RayWork.CoreComponents;

public class AdaptableDebugComponent(string label, Action onDebug) : DebugComponent
{
    public string Label = label;
    public Action? OnDebug = onDebug;
    public override void Debug() => OnDebug?.Invoke();
}