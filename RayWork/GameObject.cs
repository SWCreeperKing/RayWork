using RayWork.ECS;

namespace RayWork;

public abstract class GameObject : ComponentObject
{
    private readonly ListRegister<GameObject> _childRegister = new();

    public void Update(float deltaTime)
    {
        UpdateLoop(deltaTime);
        _childRegister.ExecuteRegister(child => child.Update(deltaTime));
    }

    public void Render()
    {
        RenderLoop();
        _childRegister.ExecuteRegister(child => child.Render());
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
}