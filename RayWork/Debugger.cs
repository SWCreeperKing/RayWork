using ImGuiNET;
using RayWork.ECS;

namespace RayWork;

public static class Debugger
{
    private static (string, Scene)[] _scenes;

    public static bool IsDebugging;

    public static void Initialize()
    {
        SceneManager.OnSceneListChanged += (_, _) => _scenes = SceneManager.GetAllScenes();
    }

    public static void Render()
    {
        if (!IsDebugging) return;
        if (!ImGui.Begin("SceneManager")) return;

        foreach (var (id, scene) in _scenes)
        {
            if (scene.HasChildren()) continue;
            RenderScene(id, scene);
        }
    }

    private static void RenderScene(string id, Scene scene)
    {
        if (!ImGui.CollapsingHeader(id)) return;
        var objects = scene.GetChildren();
        for (var i = 0; i < objects.Length; i++) RenderGameObject(objects[i], i);
    }

    private static void RenderGameObject(GameObject gameObject, int i)
    {
        var components = gameObject.GetDebugComponents();
        if (!ImGui.TreeNode($"({i + 1}) {gameObject.GetType().Name}")) return;

        if (components.Any() && ImGui.CollapsingHeader("Components"))
        {
            for (var j = 0; j < components.Length; j++) RenderComponent(components[j], j);
        }

        var objectChildren = gameObject.GetChildren();
        if (objectChildren.Any() && ImGui.CollapsingHeader("Children"))
        {
            var objects = gameObject.GetChildren();
            for (var j = 0; j < objects.Length; j++) RenderGameObject(objects[j], j);
        }

        ImGui.TreePop();
    }

    private static void RenderComponent(DebugComponent component, int i)
    {
        if (!ImGui.TreeNode($"({i + 1}) {component.GetType().Name}")) return;
        component.Debug();
        ImGui.TreePop();
    }
}