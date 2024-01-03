using RayWork.ECS;

namespace RayWork.CoreComponents;

public class AdaptableDebugComponent : IDebugComponent
{
    public Action OnDebug;

    public AdaptableDebugComponent(Action onDebug) => OnDebug = onDebug;
    public void Debug() => OnDebug?.Invoke();
}