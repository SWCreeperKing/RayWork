namespace RayWork.Objects;

public interface IScene
{
    public string Label { get; }

    public void Update();
    public void Render();

    public void Initialize();
    public void ReInitialize();
    public void UpdateLoop();
    public void RenderLoop();
    public void DebugLoop();
    public void Dispose();

    public void AddChild(GameObject gameObject);
    public void RemoveChild(GameObject gameObject);

    public GameObject[] GetChildren();
    public bool HasChildren();
}