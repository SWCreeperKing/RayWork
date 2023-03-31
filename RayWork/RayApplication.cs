using System.Numerics;
using Raylib_CsLo;
using RayWork.EventArguments;
using RayWork.RLImgui;

namespace RayWork;

public class RayApplication
{
    public static Color BackgroundColor { get; set; } = new(177, 177, 177, 255);
    public static float DeltaTime { get; private set; }

    public static Vector2 WindowSize => _windowSize;

    public static event EventHandler<WindowSizeChangedEventArgs> OnWindowSizeChanged;

    private static long _lastUpdate;
    private static Vector2 _windowSize;

    public RayApplication(Scene mainScene, Vector2 windowSize, string title = "Untitled",
        int fps = 60, ConfigFlags configFlags = 0)
    {
        Raylib.SetConfigFlags(configFlags);
        Raylib.SetTargetFPS(fps);
        _windowSize = windowSize;

        Logger.Initialize();
        Debugger.Initialize();
        Raylib.InitWindow((int) windowSize.X, (int) windowSize.Y, title);
        SceneManager.AddScene("main", mainScene);
        RlImgui.Setup(() => _windowSize);

        Start();
    }

    public RayApplication(Scene mainScene, int windowWidth, int windowHeight, string title = "Untitled", int fps = 60,
        ConfigFlags configFlags = 0) : this(mainScene, new Vector2(windowWidth, windowHeight), title, fps, configFlags)
    {
    }

    private void Start()
    {
        _lastUpdate = GetTimeMs();

        try
        {
            SceneManager.Scene.Initialize();
            RunRayLoop();
            Dispose();
        }
        catch (Exception e)
        {
            Logger.Log(e);
            Dispose();
            Console.WriteLine(
                "\n\nPROGRAM HAS CRASHED\nPROGRAM IS PAUSED TO ENCOURAGE ERROR REPORT\nPress enter key to continue . . .");
            Console.ReadLine();
        }
    }

    private void RunRayLoop()
    {
        while (!Raylib.WindowShouldClose())
        {
            Update();
            Render();
        }
    }

    private void Update()
    {
        var currentWindowSize = new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
        var currentTimeMs = GetTimeMs();
        DeltaTime = (currentTimeMs - _lastUpdate) / 1000f;

        if (currentWindowSize != _windowSize)
        {
            var windowSizeChangeArgs = new WindowSizeChangedEventArgs(_windowSize = currentWindowSize);
            if (OnWindowSizeChanged is not null)
            {
                OnWindowSizeChanged(null, windowSizeChangeArgs);
            }
        }

        Input.UpdateInput(DeltaTime);
        SceneManager.Scene.Update();

        _lastUpdate = currentTimeMs;
    }

    private void Render()
    {
        Raylib.BeginDrawing();
        RlImgui.Begin();
        Raylib.ClearBackground(BackgroundColor);

        SceneManager.Scene.Render();
        Debugger.Render();

        RlImgui.End();
        Raylib.EndDrawing();
    }

    private void Dispose()
    {
        RlImgui.Shutdown();
        SceneManager.DisposeScenes();
        Logger.CheckWrite();
    }

    public static long GetTimeMs() => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
}