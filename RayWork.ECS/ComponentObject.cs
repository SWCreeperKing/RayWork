namespace RayWork.ECS;

public class ComponentObject
{
    private readonly ListRegister<Component> _componentRegister = new();
    private DebugComponent[] _debugComponents;

    public ComponentObject()
    {
        _componentRegister.OnRegisterCacheUpdated += (_, _) =>
        {
            _debugComponents = GetAllComponents().OfType<DebugComponent>().ToArray();
        };
    }

    public void UpdateRegister()
    {
        _componentRegister.UpdateRegister();
    }
    
    public void AddComponent(Component componentToAdd)
    {
        _componentRegister.AddToRegister(componentToAdd);
    }

    public void RemoveComponent(Component componentToRemove)
    {
        _componentRegister.RemoveFromRegister(componentToRemove);
    }

    public bool ContainsComponent<ComponentType>() where ComponentType : Component
    {
        return _componentRegister.RegisterContainsType<ComponentType>();
    }

    public ComponentType GetComponent<ComponentType>() where ComponentType : Component
    {
        return _componentRegister.GetTypeFromRegister<ComponentType>();
    }
    
    public ComponentType[] GetComponents<ComponentType>() where ComponentType : Component
    {
        return _componentRegister.GetTypesFromRegister<ComponentType>();
    }

    public DebugComponent[] GetDebugComponents()
    {
        return _debugComponents;
    }
    
    public Component[] GetAllComponents()
    {
        return _componentRegister.GetRegisterTypes();
    } 
}