using System.Numerics;
using RayWork.ECS;

namespace RayWork.CoreComponents;

public abstract class TransformComponent : DebugComponent
{
    public Vector2 position;

    public virtual void Debug()
    {
    }
}