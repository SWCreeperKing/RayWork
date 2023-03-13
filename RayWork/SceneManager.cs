namespace RayWork;

public class SceneManager
{
    private readonly Dictionary<string, Scene> _scenes = new();

    public void AddScene(string id, Scene scene)
    {
        if (_scenes.ContainsKey(id)) throw new AggregateException($"SceneManager already contains ID: [{id}]");
        
        _scenes[id] = scene;
    }
    
    
}