namespace RayWork.ECS;

public class ListRegister<RegisterType>
{
    private readonly List<RegisterType> _register = new();
    private readonly List<RegisterType> _registerAddQueue = new();
    private readonly List<RegisterType> _registerRemoveQueue = new();

    public void ExecuteRegister(Action<RegisterType> registerAction)
    {
        for (var i = 0; i < _register.Count; i++)
        {
            registerAction(_register[i]);
        }
    }

    public void UpdateRegister()
    {
        if (_registerAddQueue.Any())
        {
            _register.AddRange(_registerAddQueue);
            _registerAddQueue.Clear();
        }
        
        if (!_registerRemoveQueue.Any()) return;

        for (var i = 0; i < _registerRemoveQueue.Count; i++)
        {
            _register.Remove(_registerRemoveQueue[i]);
        }
    }
    
    public void AddToRegister(RegisterType objectToRegister) 
    {
        _registerAddQueue.Add(objectToRegister);
    }

    public void RemoveFromRegister(RegisterType objectToRemove)
    {
        _registerRemoveQueue.Add(objectToRemove);
    }

    public bool RegisterContainsType<TypeToCheck>() where TypeToCheck : RegisterType
    {
        return _register.OfType<TypeToCheck>().Any();
    }

    public TypeToGet GetTypeFromRegister<TypeToGet>() where TypeToGet : RegisterType
    {
        return _register.OfType<TypeToGet>().First();
    }

    public TypeToGet[] GetTypesFromRegister<TypeToGet>() where TypeToGet : RegisterType
    {
        return _register.OfType<TypeToGet>().ToArray();
    }

    public RegisterType[] GetRegisterTypes()
    {
        return _register.ToArray();
    }

    public bool IsRegisterEmpty()
    {
        return _register.Any();
    }
}