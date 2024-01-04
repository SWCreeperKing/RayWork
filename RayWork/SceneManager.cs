using RayWork.Objects;

namespace RayWork;

public static class SceneManager
{
    private static readonly Dictionary<string, Scene> Scenes = new();
    private static readonly List<string> InitializedScenes = [];
    private static string ActiveSceneId = "main";

    public static event EventHandler? OnSceneListChanged;

    public static Scene Scene
    {
        get
        {
            if (!Scenes.TryGetValue(ActiveSceneId, out var value))
                throw new ArgumentException($"The current scene ID [{ActiveSceneId}] does not exist");

            return value;
        }
    }

    public static void AddScene(string id, Scene scene)
    {
        if (!Scenes.TryAdd(id, scene)) throw new ArgumentException($"SceneManager already contains ID: [{id}]");
        SceneListChanged();
    }

    public static void RemoveScene(string id, out Scene removedScene)
    {
        if (!Scenes.TryGetValue(id, out var value))
            throw new ArgumentException($"SceneManager can not remove the ID: [{id}] because it doesn't exist");

        removedScene = value;
        Scenes.Remove(id);
        InitializedScenes.Remove(id);
        SceneListChanged();
    }

    public static void DisposeScenes()
    {
        try
        {
            foreach (var id in InitializedScenes)
            {
                Scenes[id].DisposeLoop();
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
        if (!Scenes.ContainsKey(id))
            throw new ArgumentException($"The current scene ID [{ActiveSceneId}] does not exist");

        ActiveSceneId = id;
        if (!InitializedScenes.Contains(id))
        {
            Scene.Initialize();
            InitializedScenes.Add(id);
        }
        else
        {
            Scene.ReInitialize();
        }
    }

    public static (string, Scene)[] GetAllScenes() => Scenes.Select(kv => (kv.Key, kv.Value)).ToArray();
    private static void SceneListChanged() => OnSceneListChanged?.Invoke(null, null);
}