using Raylib_CsLo;
using RayWork;
using RayWork.CoreComponents;

namespace RayWorkTester;

public class TestObject : GameObject
{
    private PositionComponent Pos;
    private SizeComponent Size;
    private ColorComponent Color;

    public TestObject()
    {
        AddComponent(Pos = new(200, 100));
        AddComponent(Size = new StaticSizeComponent(50, 50));
        AddComponent(Color = new(Raylib.RED));
    }

    public override void RenderLoop() => Raylib.DrawRectangleV(Pos.Position, Size.Size, Color.Color);
}