namespace RayWork.ECS;

public class ListRegister<TRegisterType>
{
    private readonly List<TRegisterType> Register = [];
    private readonly List<TRegisterType> RegisterAddQueue = [];
    private readonly List<TRegisterType> RegisterRemoveQueue = [];

    private TRegisterType[] CachedRegister = [];

    public event EventHandler? OnRegisterCacheUpdated;

    public int Count { get; private set; }

    public void ExecuteRegister(Action<TRegisterType> registerAction)
    {
        if (CachedRegister.Length < 1) return;
        foreach (var t in CachedRegister) registerAction(t);
    }

    public void UpdateRegister()
    {
        if (RegisterAddQueue.Count != 0)
        {
            Register.AddRange(RegisterAddQueue);
            RegisterAddQueue.Clear();
            UpdateRegisterCache();
        }

        if (RegisterRemoveQueue.Count == 0)
        {
            Count = Register.Count;
            return;
        }

        foreach (var t in RegisterRemoveQueue)
        {
            Register.Remove(t);
            UpdateRegisterCache();
        }

        Count = Register.Count;
    }

    public void AddToRegister(TRegisterType objectToRegister) => RegisterAddQueue.Add(objectToRegister);
    public void RemoveFromRegister(TRegisterType objectToRemove) => RegisterRemoveQueue.Add(objectToRemove);

    public bool RegisterContainsType<TYpeToCheck>() where TYpeToCheck : TRegisterType
        => Register.OfType<TYpeToCheck>().Any();

    public TYpeToGet GetTypeFromRegister<TYpeToGet>() where TYpeToGet : TRegisterType
        => Register.OfType<TYpeToGet>().First();

    public TYpeToGet[] GetTypesFromRegister<TYpeToGet>() where TYpeToGet : TRegisterType
        => Register.OfType<TYpeToGet>().ToArray();

    public TRegisterType[] GetRegisterTypes() => CachedRegister;
    public bool IsRegisterEmpty() => Count != 0;

    public void UpdateRegisterCache()
    {
        CachedRegister = Register.ToArray();
        OnRegisterCacheUpdated?.Invoke(this, null!);
    }
}