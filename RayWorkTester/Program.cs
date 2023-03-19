using System.Numerics;
using Raylib_CsLo;
using RayWork;
using RayWork.Objects;

new RayApplication(new RayWorkTester.Program(), 1280, 720, "Test", 60, ConfigFlags.FLAG_WINDOW_RESIZABLE);

namespace RayWorkTester
{
    public class Program : Scene
    {
        public const string LoremIpsum =
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras aliquet dapibus metus a faucibus. Pellentesque a libero at ex gravida gravida vitae nec elit. Duis at orci lobortis, tincidunt odio nec, suscipit sapien. Interdum et malesuada fames ac ante ipsum primis in faucibus. Sed in sem volutpat metus molestie placerat a quis purus. Proin vitae orci euismod, efficitur nunc et, suscipit dui. Vivamus malesuada suscipit sapien non gravida. Vestibulum dignissim turpis sed quam aliquet porta. Aliquam euismod posuere lorem, ut aliquam tellus porta non. Nulla sit amet aliquam dui. Nunc vel suscipit odio. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos.";

        public string text;

        public override void Initialize()
        {
            SceneManager.AddScene("simon", new SimonSays());
            Button button = new("Test", new Vector2(500));
            button.OnButtonPressed += (_, _) => SceneManager.SwitchScene("simon");

            AddChild(new TestObject());
            AddChild(new AnchorTestObject());
            AddChild(new TextBlock(LoremIpsum, new Rectangle(200, 300, 300, 60)));
            AddChild(button);

            Input.OnKeyPressed += (_, keyArgs) =>
            {
                if (keyArgs.key is not KeyboardKey.KEY_F3) return;
                Debugger.IsDebugging = !Debugger.IsDebugging;
            };

            Input.OnKeyRepeat += (_, keyArgs) =>
            {
                if (keyArgs.key is KeyboardKey.KEY_A) text += "a";
                if (keyArgs.key is KeyboardKey.KEY_BACKSPACE && text.Any()) text = text[..^1];
            };
        }

        public override void RenderLoop()
        {
            Raylib.DrawFPS(0, 0);
            Raylib.DrawText(text, 0, 0, 24, Raylib.RED);
        }
    }
}