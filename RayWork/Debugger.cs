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
        foreach (var child in scene.GetChildren()) RenderGameObject(child);
    }

    private static void RenderGameObject(GameObject gameObject)
    {
        var components = gameObject.GetAllComponents().OfType<DebugComponent>();
        if (!components.Any()) return;
        if (!ImGui.TreeNode(gameObject.GetType().Name)) return;

        foreach (var component in components) RenderComponent(component);
        ImGui.TreePop();
    }

    private static void RenderComponent(DebugComponent component)
    {
        if (!ImGui.TreeNode(component.GetType().Name)) return;
        component.Debug();
        ImGui.TreePop();
    }
}