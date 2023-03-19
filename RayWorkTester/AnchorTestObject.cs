using System.Numerics;
using Raylib_CsLo;
using RayWork;
using RayWork.CoreComponents;
using RayWork.Objects;

namespace RayWorkTester;

public class AnchorTestObject : GameObject
{
    private ScreenAnchorComponent _pos;
    private SizeComponent _size;
    private ColorComponent _color;

    public AnchorTestObject()
    {
        AddComponent(_size = new StaticSizeComponent(50, 50));
        AddComponent(_pos = new ScreenAnchorComponent(windowSize =>
            new Vector2(windowSize.X / 2 - _size.Size.X / 2, windowSize.Y - _size.Size.Y - 20)));
        AddComponent(_color = new ColorComponent(Raylib.RED));
        AddChild(new Text("Testing string", new Vector2(100, 50)));
    }

    public override void RenderLoop()
    {
        Raylib.DrawRectangleV(_pos.Position, _size.Size, _color.color);
    }
}