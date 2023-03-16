using System.Numerics;
using Raylib_CsLo;
using RayWork;
using RayWork.Objects;

new RayApplication(new RayWorkTester.Program(), 1280, 720, "Test", ConfigFlags.FLAG_WINDOW_RESIZABLE);

namespace RayWorkTester
{
    public class Program : Scene
    {
        public const string LoremIpsum =
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras aliquet dapibus metus a faucibus. Pellentesque a libero at ex gravida gravida vitae nec elit. Duis at orci lobortis, tincidunt odio nec, suscipit sapien. Interdum et malesuada fames ac ante ipsum primis in faucibus. Sed in sem volutpat metus molestie placerat a quis purus. Proin vitae orci euismod, efficitur nunc et, suscipit dui. Vivamus malesuada suscipit sapien non gravida. Vestibulum dignissim turpis sed quam aliquet porta. Aliquam euismod posuere lorem, ut aliquam tellus porta non. Nulla sit amet aliquam dui. Nunc vel suscipit odio. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos.";

        public override void Initialize()
        {
            AddChild(new TestObject());
            AddChild(new AnchorTestObject());
            AddChild(new Text("Testing string", new Vector2(100, 10)));
            AddChild(new TextBlock(LoremIpsum, new Rectangle(200, 300, 300, 60), Raylib.BLACK));
        }

        public override void UpdateLoop(float deltaTime)
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_F3)) Debugger.IsDebugging = !Debugger.IsDebugging;
        }
    }
}