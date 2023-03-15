namespace RayWork;

public static class SceneManager
{
    private static readonly Dictionary<string, Scene> _scenes = new();
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
        SceneListChanged();
    }

    public static void SwitchScene(string id)
    {
        if (!_scenes.ContainsKey(id))
        {
            throw new ArgumentException($"The current scene ID [{_activeSceneId}] does not exist");
        }

        _activeSceneId = id;
        Scene.Initialize();
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