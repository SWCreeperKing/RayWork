namespace RayWork.ECS;

public class ComponentObject
{
    private readonly ListRegister<UpdateComponent> _updateRegister = new();
    private readonly ListRegister<RenderComponent> _renderRegister = new();

    public void Update(float dt)
    {
        _updateRegister.ExecuteRegister(component => component.Update(dt));
    }

    public void Render()
    {
        _renderRegister.ExecuteRegister(component => component.Render());
    }

    public void AddComponent(Component componentToAdd)
    {
        if (componentToAdd is UpdateComponent updateComponent)
        {
            _updateRegister.AddToRegister(updateComponent);
        }

        if (componentToAdd is RenderComponent renderComponent)
        {
            _renderRegister.AddToRegister(renderComponent);
        }
    }
    
    public void RemoveComponent(Component componentToAdd)
    {
        if (componentToAdd is UpdateComponent updateComponent)
        {
            _updateRegister.RemoveFromRegister(updateComponent);
        }

        if (componentToAdd is RenderComponent renderComponent)
        {
            _renderRegister.RemoveFromRegister(renderComponent);
        }
    }
}