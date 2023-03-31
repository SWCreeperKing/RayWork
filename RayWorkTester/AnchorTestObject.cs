using System.Numerics;
using Raylib_CsLo;
using RayWork;
using RayWork.CoreComponents;
using RayWork.Objects;

namespace RayWorkTester;

public class AnchorTestObject : GameObject
{
    private ScreenAnchorComponent Pos;
    private SizeComponent Size;
    private ColorComponent Color;

    public AnchorTestObject()
    {
        AddComponent(Size = new StaticSizeComponent(50, 50));
        AddComponent(Pos = new(windowSize =>
            new(windowSize.X / 2 - Size.Size.X / 2, windowSize.Y - Size.Size.Y - 20)));
        AddComponent(Color = new(Raylib.RED));
        AddChild(new Text("Testing string", new Vector2(100, 50)));
    }

    public override void RenderLoop() => Raylib.DrawRectangleV(Pos.Position, Size.Size, Color.Color);
}