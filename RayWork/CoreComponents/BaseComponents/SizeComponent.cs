using System.Numerics;
using RayWork.ECS;

namespace RayWork.CoreComponents;

public abstract class SizeComponent : IDebugComponent
{
    public abstract Vector2 Size { get; set; }

    public virtual void Debug()
    {
    }
}