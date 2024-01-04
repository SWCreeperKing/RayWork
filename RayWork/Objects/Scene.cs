using RayWork.ECS;

namespace RayWork.Objects;

public abstract class Scene
{
    private readonly ListRegister<GameObject> ChildRegister = new();
    public abstract string Label { get; }

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

    public virtual void DisposeLoop()
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