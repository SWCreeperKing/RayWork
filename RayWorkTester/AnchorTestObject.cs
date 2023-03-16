using System.Numerics;
using Raylib_CsLo;
using RayWork;
using RayWork.CoreComponents;

namespace RayWorkTester;

public class AnchorTestObject : GameObject
{
    private AnchorComponent _pos;
    private SizeComponent _size;
    private ColorComponent _color;

    public AnchorTestObject()
    {
        AddComponent(_size = new SizeComponent(50, 50));
        AddComponent(_pos = new AnchorComponent(windowSize =>
            new Vector2(windowSize.X / 2 - _size.size.X / 2, windowSize.Y - _size.size.Y - 20)));
        AddComponent(_color = new ColorComponent(Raylib.RED));
    }

    public override void RenderLoop()
    {
        Raylib.DrawRectangleV(_pos.position, _size.size, _color.color);
    }
}