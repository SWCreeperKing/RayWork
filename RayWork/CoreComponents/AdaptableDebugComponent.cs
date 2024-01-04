using RayWork.ECS;

namespace RayWork.CoreComponents;

public class AdaptableDebugComponent(Action onDebug) : IDebugComponent
{
    public Action? OnDebug = onDebug;
    public void Debug() => OnDebug?.Invoke();
}