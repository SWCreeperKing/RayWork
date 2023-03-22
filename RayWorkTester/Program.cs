using System.Numerics;
using Raylib_CsLo;
using RayWork;
using RayWork.Objects;
using RayWorkTester;

new RayApplication(new Program(), 1280, 720, "Test", 60, ConfigFlags.FLAG_WINDOW_RESIZABLE);

public partial class Program : Scene
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

        NumberClass number = new(52);
        Logger.Log(number.ToLongString());

        NumberClass number2 = new(520000);
        Logger.Log(number2.ToLongString());

        var number3 = number + number2;
        Logger.Log(number3.ToLongString());

        var number4 = number - number3;
        Logger.Log(number4.ToLongString());

        var number5 = number3 - number;
        Logger.Log(number5.ToLongString());

        var number6 = number * number2;
        Logger.Log(number6.ToLongString());

        var number7 = number / number2;
        Logger.Log(number7.ToString(false));

        var number8 = number2 / number;
        Logger.Log(number8.ToLongString());

        NumberClass number9 = new("1e1e10");
        Logger.Log(number9.ToLongString());

        NumberClass number10 = new("1ee7");
        Logger.Log(number10.ToLongString());
    }

    public override void RenderLoop()
    {
        Raylib.DrawFPS(0, 0);
        Raylib.DrawText(text, 0, 0, 24, Raylib.RED);
    }
}