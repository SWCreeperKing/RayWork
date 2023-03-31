using System.Numerics;
using RayWork.ECS;

namespace RayWork.CoreComponents;

public abstract class TransformComponent : DebugComponent
{
    public abstract Vector2 Position { get; set; }

    public virtual void Debug()
    {
    }
}