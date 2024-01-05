using System.Numerics;
using ImGuiNET;
using Raylib_cs;
using RayWork;
using RayWork.ComponentObjects;
using RayWork.Objects;

namespace RayWorkTester;

public class MainScene : Scene
{
    public const string LoremIpsum =
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras aliquet dapibus metus a faucibus. Pellentesque a libero at ex gravida gravida vitae nec elit. Duis at orci lobortis, tincidunt odio nec, suscipit sapien. Interdum et malesuada fames ac ante ipsum primis in faucibus. Sed in sem volutpat metus molestie placerat a quis purus. Proin vitae orci euismod, efficitur nunc et, suscipit dui. Vivamus malesuada suscipit sapien non gravida. Vestibulum dignissim turpis sed quam aliquet porta. Aliquam euismod posuere lorem, ut aliquam tellus porta non. Nulla sit amet aliquam dui. Nunc vel suscipit odio. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos.";

    public override string Label => "main";

    public override void Initialize()
    {
        SceneManager.AddScene(new SimonSays.SimonSays());

        Button button = new("Test", new Vector2(500));
        button.OnButtonPressed += (_, _) => SceneManager.SwitchScene("simon");

        AddChild(new TestObject());
        AddChild(new AnchorTestObject());
        AddChild(new TextBlock(LoremIpsum, new Rectangle(200, 300, 300, 60)));
        AddChild(new InputBox(new Vector2(300, 20), new Vector2(300, 30)));
        AddChild(button);
    }

    public override void RenderLoop() => Raylib.DrawFPS(0, 0);
    public override void DebugLoop() => ImGui.Text("Testing Custom Debug Info!");
}