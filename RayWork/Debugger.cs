using ImGuiNET;
using RayWork.CoreComponents;
using RayWork.ECS;
using RayWork.Objects;

namespace RayWork;

public static class Debugger
{
    private static Scene[] Scenes = [];

    public static bool IsDebugging;

    public static void Initialize() => SceneManager.OnSceneListChanged += () => Scenes = SceneManager.GetAllScenes();

    public static void Render(RayApplication app)
    {
        if (!IsDebugging) return;
        ImGui.ShowMetricsWindow();
        RenderSceneManager(app);
        RenderDebugger(app);
    }

    private static void RenderSceneManager(RayApplication app)
    {
        if (!ImGui.Begin("SceneManager")) return;
        app.ManagerLoop();
        ImGui.Separator();

        foreach (var scene in Scenes)
        {
            if (scene.HasChildren()) continue;
            RenderScene(scene);
        }
    }

    private static void RenderDebugger(RayApplication app)
    {
        if (!ImGui.Begin("Debugger")) return;
        app.DebugLoop();
        ImGui.Separator();

        foreach (var scene in Scenes)
        {
            if (scene.HasChildren()) continue;
            if (!ImGui.CollapsingHeader(scene.Label)) continue;
            scene.DebugLoop();

            var objects = scene.GetChildren();

            for (var i = 0; i < objects.Length; i++)
            {
                var gameObject = objects[i];
                if (!ImGui.TreeNode(gameObject.Id, $"({i + 1}) {gameObject.GetType().Name}")) continue;
                gameObject.DebugLoop();
                ImGui.TreePop();
            }
        }
    }

    private static void RenderScene(Scene scene)
    {
        if (!ImGui.CollapsingHeader(scene.Label)) return;
        var objects = scene.GetChildren();

        for (var i = 0; i < objects.Length; i++)
        {
            RenderGameObject(objects[i], i);
        }
    }

    private static void RenderGameObject(GameObject gameObject, int i)
    {
        var components = gameObject.GetDebugComponents();
        if (!ImGui.TreeNode(gameObject.Id, $"({i + 1}) {gameObject.GetType().Name}")) return;

        if (components.Length != 0 && ImGui.CollapsingHeader("Components"))
        {
            for (var j = 0; j < components.Length; j++)
            {
                RenderComponent(components[j], j);
            }
        }

        var objectChildren = gameObject.GetChildren();
        if (objectChildren.Length != 0 && ImGui.CollapsingHeader("Children"))
        {
            var objects = gameObject.GetChildren();
            for (var j = 0; j < objects.Length; j++)
            {
                RenderGameObject(objects[j], j);
            }
        }

        ImGui.TreePop();
    }

    private static void RenderComponent(DebugComponent component, int i)
    {
        var name = component is AdaptableDebugComponent adc ? adc.Label : component.GetType().Name;
        if (!ImGui.TreeNode(component.Id, $"({i + 1}) {name}")) return;
        component.Debug();
        ImGui.TreePop();
    }

    public static void ToggleDebugger() => IsDebugging = !IsDebugging;
}