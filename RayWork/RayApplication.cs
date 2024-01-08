using System.Numerics;
using ImGuiNET;
using Raylib_cs;
using RayWork.Commands;
using RayWork.CoreComponents.BaseComponents;
using RayWork.EventArguments;
using RayWork.Objects;
using RayWork.RLImgui;

namespace RayWork;

public class RayApplication
{
    public static CompatibleColor BackgroundColor { get; set; } = new(177);
    public static Vector2 WindowSize { get; private set; }
    public static KeyboardKey? ToggleDebuggerKey { get; set; } = KeyboardKey.KEY_F3;
    public static KeyboardKey? ToggleConsoleKey { get; set; } = KeyboardKey.KEY_GRAVE;
    public static float DeltaTime { get; private set; }
    public static string WindowTitle;
    public static bool PrintLogToWindowsConsole;
    public static bool UseCascadiaAsImguiFont = true;

    public static event EventHandler<WindowSizeChangedEventArgs>? OnWindowSizeChanged;

    private static long LastUpdate;

    public RayApplication(IScene mainScene, Vector2 windowSize, string title = "Untitled",
        int fps = 60, ConfigFlags configFlags = 0, bool enableImguiDocking = false)
    {
        if (mainScene.Label is not "main")
            throw new ArgumentException($"the mainScene: [{mainScene.GetType()}]'s label is not 'main'");

        Logger.Initialize();
        Raylib.SetConfigFlags(configFlags);
        Raylib.SetTargetFPS(fps);
        WindowSize = windowSize;

        Debugger.Initialize();
        CommandRegister.RegisterCommandFile(typeof(DefaultCommands));
        Raylib.InitWindow((int) windowSize.X, (int) windowSize.Y, WindowTitle = title);
        SceneManager.AddScene(mainScene);
        RlImgui.Setup(enableDocking: enableImguiDocking);

        Start();
    }

    public RayApplication(IScene mainScene, int windowWidth, int windowHeight, string title = "Untitled",
        int fps = 60, ConfigFlags configFlags = 0, bool enableImguiDocking = false)
        : this(mainScene, new Vector2(windowWidth, windowHeight), title, fps, configFlags, enableImguiDocking)
    {
    }

    private void Start()
    {
        LastUpdate = GetTimeMs();
        FontComponent.DefaultFont = RlImgui.LoadCascadiaCode();

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
        DeltaTime = (currentTimeMs - LastUpdate) / 1000f;

        if (currentWindowSize != WindowSize)
        {
            var windowSizeChangeArgs = new WindowSizeChangedEventArgs(WindowSize = currentWindowSize);
            OnWindowSizeChanged?.Invoke(null, windowSizeChangeArgs);
        }

        Input.UpdateInput(DeltaTime);

        if (ToggleDebuggerKey is not null && Input.IsKeyPressed(ToggleDebuggerKey.Value))
        {
            Debugger.ToggleDebugger();
        }

        if (ToggleConsoleKey is not null && Input.IsKeyPressed(ToggleConsoleKey.Value))
        {
            GameConsole.ToggleConsole();
        }

        SceneManager.Scene.Update();

        LastUpdate = currentTimeMs;
    }

    private void Render()
    {
        Raylib.BeginDrawing();
        RlImgui.Begin();

        if (UseCascadiaAsImguiFont)
        {
            ImGui.PushFont(RlImgui.CascadiaCode);
        }

        Raylib.ClearBackground(BackgroundColor);

        SceneManager.Scene.Render();
        Debugger.Render(this);
        GameConsole.Render();

        RlImgui.End();
        Raylib.EndDrawing();
    }

    public void ManagerLoop()
    {
        BackgroundColor.ImGuiColorEdit("Background Color");

        ImGui.Checkbox("Log to Windows Console? ", ref PrintLogToWindowsConsole);
        ImGui.Checkbox("Use Cascadia Code as Default Imgui Font? ", ref UseCascadiaAsImguiFont);

        // var fontSize = ImGui.GetFontSize();
        // ImGui.InputInt("Imgui Font Size", ref ImGui.font)

        if (!ImGui.InputText("Window Title", ref WindowTitle, 512)) return;
        Raylib.SetWindowTitle(WindowTitle);
    }

    public void DebugLoop()
        => ImGui.Text($"""
                       Delta Time: {DeltaTime}
                       Window Size: {WindowSize}
                       Toggle Debugger: {ToggleDebuggerKey}
                       Toggle Console: {ToggleConsoleKey}
                       """);

    private void Dispose()
    {
        RlImgui.Shutdown();
        SceneManager.DisposeScenes();
        Logger.CheckWrite();
    }

    public static long GetTimeMs() => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
}