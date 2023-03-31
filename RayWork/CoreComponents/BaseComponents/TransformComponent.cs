using System.Numerics;
using RayWork.ECS;

namespace RayWork.CoreComponents;

public abstract class TransformComponent : IDebugComponent
{
    public abstract Vector2 Position { get; set; }

    public virtual void Debug()
    {
    }
}