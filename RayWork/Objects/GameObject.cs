using Raylib_CsLo;
using RayWork.ECS;

namespace RayWork;

public abstract class GameObject : ComponentObject
{
    public object Parent { get; set; }
    
    private readonly ListRegister<GameObject> _childRegister = new();

    public void Update()
    {
        UpdateRegister();
        _childRegister.UpdateRegister();
        UpdateLoop();
        _childRegister.ExecuteRegister(child => child.Update());
    }

    public void Render()
    {
        RenderLoop();
        _childRegister.ExecuteRegister(child => child.Render());
    }
    
    public virtual void UpdateLoop()
    {
    }

    public virtual void RenderLoop()
    {
    }

    public virtual void DebugLoop()
    {
    }

    public virtual MouseCursor OccupiedMouseCursor()
    {
        return MouseCursor.MOUSE_CURSOR_DEFAULT;
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
}