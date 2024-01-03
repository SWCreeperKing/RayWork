using Raylib_cs;
using RayWork;
using RayWork.CoreComponents;
using static Raylib_cs.Color;

namespace RayWorkTester;

public class TestObject : GameObject
{
    private PositionComponent Pos;
    private SizeComponent Size;
    private ColorComponent Color;

    public TestObject()
    {
        AddComponent(Pos = new PositionComponent(200, 100));
        AddComponent(Size = new StaticSizeComponent(50, 50));
        AddComponent(Color = new ColorComponent(RED));
    }

    public override void RenderLoop() => Raylib.DrawRectangleV(Pos.Position, Size.Size, Color.Color);
}