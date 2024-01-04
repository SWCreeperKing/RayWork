using System.Numerics;
using RayWork.ECS;

namespace RayWork.CoreComponents.BaseComponents;

public abstract class SizeComponent : DebugComponent
{
    public abstract Vector2 Size { get; set; }
}