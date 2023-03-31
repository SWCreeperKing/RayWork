namespace RayWork.ECS;

public class ListRegister<TRegisterType>
{
    private readonly List<TRegisterType> Register = new();
    private readonly List<TRegisterType> RegisterAddQueue = new();
    private readonly List<TRegisterType> RegisterRemoveQueue = new();

    private TRegisterType[] CachedRegister = Array.Empty<TRegisterType>();

    public event EventHandler OnRegisterCacheUpdated;

    public void ExecuteRegister(Action<TRegisterType> registerAction)
    {
        if (CachedRegister is null || CachedRegister.Length < 0) return;
        for (var i = 0; i < CachedRegister.Length; i++)
        {
            registerAction(CachedRegister[i]);
        }
    }

    public void UpdateRegister()
    {
        if (RegisterAddQueue.Any())
        {
            Register.AddRange(RegisterAddQueue);
            RegisterAddQueue.Clear();
            UpdateRegisterCache();
        }

        if (!RegisterRemoveQueue.Any()) return;

        for (var i = 0; i < RegisterRemoveQueue.Count; i++)
        {
            Register.Remove(RegisterRemoveQueue[i]);
            UpdateRegisterCache();
        }
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
    public bool IsRegisterEmpty() => Register.Any();

    public void UpdateRegisterCache()
    {
        CachedRegister = Register.ToArray();

        if (OnRegisterCacheUpdated is not null)
        {
            OnRegisterCacheUpdated(null, null);
        }
    }
}