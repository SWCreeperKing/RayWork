using Raylib_CsLo;
using RayWork.ECS;

namespace RayWork;

public abstract class GameObject : ComponentObject
{
    public object Parent { get; set; }

    private readonly ListRegister<GameObject> ChildRegister = new();

    public void Update()
    {
        UpdateRegister();
        ChildRegister.UpdateRegister();
        UpdateLoop();
        ChildRegister.ExecuteRegister(child => child.Update());
    }

    public void Render()
    {
        RenderLoop();
        ChildRegister.ExecuteRegister(child => child.Render());
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

    public virtual MouseCursor OccupiedMouseCursor() => MouseCursor.MOUSE_CURSOR_DEFAULT;

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
}