namespace RayWork.ECS;

public class ComponentObject
{
    private readonly ListRegister<IComponent> ComponentRegister = new();
    private DebugComponent[] DebugComponents = [];

    public int ComponentCount => ComponentRegister.Count;
    public int DebugComponentCount => DebugComponents.Length;
    
    public ComponentObject()
    {
        ComponentRegister.OnRegisterCacheUpdated += (_, _) =>
        {
            DebugComponents = GetAllComponents().OfType<DebugComponent>().ToArray();
        };
    }

    public void UpdateRegister() => ComponentRegister.UpdateRegister();
    public void AddComponent(IComponent componentToAdd) => ComponentRegister.AddToRegister(componentToAdd);

    public void RemoveComponent(IComponent componentToRemove)
        => ComponentRegister.RemoveFromRegister(componentToRemove);

    public bool ContainsComponent<TComponentType>() where TComponentType : IComponent
        => ComponentRegister.RegisterContainsType<TComponentType>();

    public TComponentType GetComponent<TComponentType>() where TComponentType : IComponent
        => ComponentRegister.GetTypeFromRegister<TComponentType>();

    public TComponentType[] GetComponents<TComponentType>() where TComponentType : IComponent
        => ComponentRegister.GetTypesFromRegister<TComponentType>();

    public DebugComponent[] GetDebugComponents() => DebugComponents;
    public IComponent[] GetAllComponents() => ComponentRegister.GetRegisterTypes();
}