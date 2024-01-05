using ImGuiNET;
using RayWork.ECS;

namespace RayWork.Objects;

public abstract class Scene
{
    private static int IncrementedId;
    public readonly int Id;

    private readonly ListRegister<GameObject> ChildRegister = new();
    public abstract string Label { get; }

    public Scene() => Id = IncrementedId++;

    public void Update()
    {
        ChildRegister.UpdateRegister();
        UpdateLoop();
        ChildRegister.ExecuteRegister(child => child.Update());
    }

    public void Render()
    {
        RenderLoop();
        ChildRegister.ExecuteRegister(child => child.Render());
    }

    public virtual void Initialize()
    {
    }

    public virtual void ReInitialize()
    {
    }

    public virtual void UpdateLoop()
    {
    }

    public virtual void RenderLoop()
    {
    }

    public virtual void DebugLoop()
        => ImGui.Text($"""
                       Scene ID: [{Id}]
                       Scene Label: [{Label}]
                       Children ComponentCount: [{ChildRegister.Count}]
                       """);

    public virtual void Dispose()
    {
    }

    public void AddChild(GameObject gameObject)
    {
        gameObject.Parent = this;
        ChildRegister.AddToRegister(gameObject);
    }

    public void RemoveChild(GameObject gameObject)
    {
        gameObject.Parent = null;
        ChildRegister.RemoveFromRegister(gameObject);
    }

    public GameObject[] GetChildren() => ChildRegister.GetRegisterTypes();
    public bool HasChildren() => !ChildRegister.IsRegisterEmpty();
}