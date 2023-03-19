using RayWork.ECS;

namespace RayWork;

public abstract class Scene
{
    private readonly ListRegister<GameObject> _childRegister = new();

    public void Update()
    {
        _childRegister.UpdateRegister();
        UpdateLoop();
        _childRegister.ExecuteRegister(child => child.Update());
    }

    public void Render()
    {
        RenderLoop();
        _childRegister.ExecuteRegister(child => child.Render());
    }
    
    public virtual void Initialize()
    {
    }

    public virtual void UpdateLoop()
    {
    }

    public virtual void RenderLoop()
    {
    }

    public void AddChild(GameObject gameObject)
    {
        gameObject.Parent = this;
        _childRegister.AddToRegister(gameObject);
    }

    public void RemoveChild(GameObject gameObject)
    {
        gameObject.Parent = null;
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