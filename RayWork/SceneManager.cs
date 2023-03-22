namespace RayWork;

public static class SceneManager
{
    private static readonly Dictionary<string, Scene> _scenes = new();
    private static readonly List<string> _initializedScenes = new();
    private static string _activeSceneId = "main";

    public static event EventHandler OnSceneListChanged;

    public static Scene Scene
    {
        get
        {
            if (!_scenes.ContainsKey(_activeSceneId))
            {
                throw new ArgumentException($"The current scene ID [{_activeSceneId}] does not exist");
            }

            return _scenes[_activeSceneId];
        }
    }

    public static void AddScene(string id, Scene scene)
    {
        if (_scenes.ContainsKey(id)) throw new ArgumentException($"SceneManager already contains ID: [{id}]");
        _scenes[id] = scene;
        SceneListChanged();
    }

    public static void RemoveScene(string id, out Scene removedScene)
    {
        if (!_scenes.ContainsKey(id))
        {
            throw new ArgumentException($"SceneManager can not remove the ID: [{id}] because it doesn't exist");
        }

        removedScene = _scenes[id];
        _scenes.Remove(id);
        if (_initializedScenes.Contains(id)) _initializedScenes.Remove(id);
        SceneListChanged();
    }

    public static void DisposeScenes()
    {
        try
        {
            foreach (var id in _initializedScenes)
            {
                _scenes[id].DisposeLoop();
            }
        }
        catch (Exception e)
        {
            Logger.Log(Logger.Level.Info, "An Error was caught during Dispose: ");
            Logger.Log(e);
        }
    }
    
    public static void SwitchScene(string id)
    {
        if (!_scenes.ContainsKey(id))
        {
            throw new ArgumentException($"The current scene ID [{_activeSceneId}] does not exist");
        }

        _activeSceneId = id;
        if (!_initializedScenes.Contains(id))
        {
            Scene.Initialize();
            _initializedScenes.Add(id);
        }
        else Scene.ReInitialize();
    }

    public static (string, Scene)[] GetAllScenes()
    {
        return _scenes.Select(kv => (kv.Key, kv.Value)).ToArray();
    }

    private static void SceneListChanged()
    {
        if (OnSceneListChanged is not null)
        {
            OnSceneListChanged(null, null);
        }
    }
}