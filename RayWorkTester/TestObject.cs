using System.Numerics;
using Raylib_CsLo;
using RayWork;
using RayWork.CoreComponents;
using Transform = RayWork.CoreComponents.Transform;

namespace RayWorkTester;

public class TestObject : GameObject
{
    private Transform _transform;
    private Size _size;
    
    public TestObject()
    {
        AddComponent(_transform = new Transform());
        AddComponent(_size = new Size() { size = new Vector2(50)});
    }

    public override void RenderLoop()
    {
        Raylib.DrawRectangleV(_transform.position, _size.size, Raylib.RED);
    }
}