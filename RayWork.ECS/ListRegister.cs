namespace RayWork.ECS;

public class ListRegister<RegisterType>
{
    private readonly List<RegisterType> _register = new();
    private readonly List<RegisterType> _registerAddQueue = new();
    private readonly List<RegisterType> _registerRemoveQueue = new();

    private RegisterType[] _cachedRegister = Array.Empty<RegisterType>();

    public event EventHandler OnRegisterCacheUpdated;

    public void ExecuteRegister(Action<RegisterType> registerAction)
    {
        if (_cachedRegister is null || _cachedRegister.Length < 0) return;
        for (var i = 0; i < _cachedRegister.Length; i++)
        {
            registerAction(_cachedRegister[i]);
        }
    }

    public void UpdateRegister()
    {
        if (_registerAddQueue.Any())
        {
            _register.AddRange(_registerAddQueue);
            _registerAddQueue.Clear();
            UpdateRegisterCache();
        }

        if (!_registerRemoveQueue.Any()) return;

        for (var i = 0; i < _registerRemoveQueue.Count; i++)
        {
            _register.Remove(_registerRemoveQueue[i]);
            UpdateRegisterCache();
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
        return _cachedRegister;
    }

    public bool IsRegisterEmpty()
    {
        return _register.Any();
    }

    public void UpdateRegisterCache()
    {
        _cachedRegister = _register.ToArray();

        if (OnRegisterCacheUpdated is not null) OnRegisterCacheUpdated(null, null);
    }
}