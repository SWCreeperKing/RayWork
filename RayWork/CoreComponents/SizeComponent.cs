using System.Numerics;
using RayWork.ECS;

namespace RayWork.CoreComponents;

public abstract class SizeComponent : DebugComponent
{
    public abstract Vector2 Size { get; set; }

    public virtual void Debug()
    {
    }
}