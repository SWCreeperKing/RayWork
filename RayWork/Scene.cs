using RayWork.ECS;

namespace RayWork;

public abstract class Scene
{
    private readonly ListRegister<GameObject> _childRegister = new();

    public void Update(float deltaTime)
    {
        _childRegister.UpdateRegister();
        UpdateLoop(deltaTime);
        _childRegister.ExecuteRegister(child => child.Update(deltaTime));
    }

    public void Render()
    {
        RenderLoop();
        _childRegister.ExecuteRegister(child => child.Render());
    }
    
    public virtual void Initialize()
    {
    }

    public virtual void UpdateLoop(float deltaTime)
    {
    }

    public virtual void RenderLoop()
    {
    }

    public void AddChild(GameObject gameObject)
    {
        _childRegister.AddToRegister(gameObject);
    }

    public void RemoveChild(GameObject gameObject)
    {
        _childRegister.RemoveFromRegister(gameObject);
    }

    public GameObject[] GetChildren()
    {
        return _childRegister.GetRegisterTypes();
    }

    public bool HasChildren()
    {
        return !_childRegister.IsRegisterEmpty();
    }
}