namespace RayWork.ECS;

public abstract class DebugComponent : IDebugComponent
{
    private static int IncrementedId;
    public readonly int Id;
    public DebugComponent() => Id = IncrementedId++;

    public virtual void Debug()
    {
    }
}