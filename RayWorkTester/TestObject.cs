using Raylib_CsLo;
using RayWork;
using RayWork.CoreComponents;

namespace RayWorkTester;

public class TestObject : GameObject
{
    private PositionComponent _pos;
    private SizeComponent _size;
    private ColorComponent _color;

    public TestObject()
    {
        AddComponent(_pos = new PositionComponent(200, 100));
        AddComponent(_size = new StaticSizeComponent(50, 50));
        AddComponent(_color = new ColorComponent(Raylib.RED));
    }

    public override void RenderLoop()
    {
        Raylib.DrawRectangleV(_pos.Position, _size.Size, _color.Color);
    }
}