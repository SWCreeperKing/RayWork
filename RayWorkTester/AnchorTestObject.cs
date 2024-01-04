using System.Numerics;
using Raylib_cs;
using RayWork;
using RayWork.ComponentObjects;
using RayWork.CoreComponents;
using RayWork.CoreComponents.BaseComponents;
using RayWork.Objects;
using static Raylib_cs.Color;

namespace RayWorkTester;

public class AnchorTestObject : GameObject
{
    private ScreenAnchorComponent Pos;
    private SizeComponent Size;
    private ColorComponent Color;

    public AnchorTestObject()
    {
        AddComponent(Size = new StaticSizeComponent(50, 50));
        AddComponent(Pos = new ScreenAnchorComponent(windowSize =>
            new Vector2(windowSize.X / 2 - Size.Size.X / 2, windowSize.Y - Size.Size.Y - 20)));
        AddComponent(Color = new ColorComponent(RED));
        AddChild(new Text("Testing string", new Vector2(100, 50)));
    }

    public override void RenderLoop() => Raylib.DrawRectangleV(Pos.Position, Size.Size, Color.Color);
}