using System.Numerics;
using Raylib_CsLo;
using RayWork.RLImgui;

namespace RayWork;

public class RayApplication
{
    public static Color BackgroundColor { get; set; } = new Color(177, 177, 177, 255);

    public static Vector2 WindowSize
    {
        get => _windowSize;
    }

    private static long _lastUpdate;
    private static Vector2 _windowSize;

    public RayApplication(Scene mainScene, int windowWidth, int windowHeight, string title = "Untitled")
    {
        _windowSize = new Vector2(windowWidth, windowHeight);

        Logger.Initialize();
        Debugger.Initialize();
        Raylib.InitWindow(windowWidth, windowHeight, title);
        SceneManager.AddScene("main", mainScene);
        RlImgui.Setup(() => _windowSize);

        Start();
    }

    private void Start()
    {
        _lastUpdate = GetTimeMs();

        try
        {
            SceneManager.Scene.Initialize();
            RunRayLoop();
        }
        catch (Exception e)
        {
            Logger.Log(e);
            Console.WriteLine(
                "\n\nPROGRAM HAS CRASHED\nPROGRAM IS PAUSED TO ENCOURAGE ERROR REPORT\nPress any key to continue");
            Console.ReadKey(true);
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
        var currentTimeMs = GetTimeMs();
        float deltaTime = currentTimeMs - _lastUpdate;
        SceneManager.Scene.Update(deltaTime);
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
    }

    public static long GetTimeMs() => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
}