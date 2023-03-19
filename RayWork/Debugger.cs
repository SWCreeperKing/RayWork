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
        var components = gameObject.GetDebugComponents();
        if (!ImGui.TreeNode(gameObject.GetType().Name)) return;

        if (components.Any() && ImGui.CollapsingHeader("Components"))
        {
            foreach (var component in components) RenderComponent(component);
        }

        var objectChildren = gameObject.GetChildren();
        if (objectChildren.Any() && ImGui.CollapsingHeader("Children"))
        {
            foreach (var child in gameObject.GetChildren()) RenderGameObject(child);
        }

        ImGui.TreePop();
    }

    private static void RenderComponent(DebugComponent component)
    {
        if (!ImGui.TreeNode(component.GetType().Name)) return;
        component.Debug();
        ImGui.TreePop();
    }
}