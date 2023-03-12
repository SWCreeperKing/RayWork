namespace RayWork.ECS;

public class ListRegister<RegisterType>
{
    private readonly List<RegisterType> _register = new();
    private readonly List<RegisterType> _registerAddQueue = new();
    private readonly List<RegisterType> _registerRemoveQueue = new();

    public void ExecuteRegister(Action<RegisterType> registerAction)
    {
        if (_registerAddQueue.Any())
        {
            _register.AddRange(_registerAddQueue);
            _registerAddQueue.Clear();
        }

        for (var i = 0; i < _register.Count; i++)
        {
            registerAction(_register[i]);
        }

        if (!_registerRemoveQueue.Any()) return;

        for (var i = 0; i < _registerRemoveQueue.Count; i++)
        {
            if (!_register.Contains(_registerRemoveQueue[i])) continue;
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
}