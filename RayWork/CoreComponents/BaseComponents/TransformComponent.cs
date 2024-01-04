using System.Numerics;
using RayWork.ECS;

namespace RayWork.CoreComponents.BaseComponents;

public abstract class TransformComponent : DebugComponent
{
    public abstract Vector2 Position { get; set; }
}