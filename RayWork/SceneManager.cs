using RayWork.Objects;
using RayWork.Objects.Primitives;

namespace RayWork;

public static class SceneManager
{
    private static readonly Dictionary<string, IScene> Scenes = new();
    private static readonly List<string> InitializedScenes = [];
    private static string ActiveSceneId = "main";

    public static event PlainEventHandler? OnSceneListChanged;

    public static IScene Scene
    {
        get
        {
            if (!Scenes.TryGetValue(ActiveSceneId, out var value))
                throw new ArgumentException($"The current scene ID [{ActiveSceneId}] does not exist");

            return value;
        }
    }

    public static void AddScene(IScene scene)
    {
        if (!Scenes.TryAdd(scene.Label, scene))
            throw new ArgumentException($"SceneManager already contains ID: [{scene.Label}]");
        SceneListChanged();
    }

    public static void RemoveScene(string id, out IScene removedScene)
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
            foreach (var id in InitializedScenes) Scenes[id].Dispose();
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
        if (InitializedScenes.Contains(id))
        {
            Scene.ReInitialize();
        }
        else
        {
            Scene.Initialize();
            InitializedScenes.Add(id);
        }
    }

    public static IScene[] GetAllScenes() => Scenes.Select(kv => kv.Value).ToArray();
    private static void SceneListChanged() => OnSceneListChanged?.Invoke();
}