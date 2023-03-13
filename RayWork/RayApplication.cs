using Raylib_CsLo;
using RayWrapper.Base.GameBox;

namespace RayWork;

public class RayApplication
{
    private long _lastUpdate;

    public RayApplication(int windowWidth, int windowHeight, string title = "Untitled")
    {
        Raylib.InitWindow(windowWidth, windowHeight, title);
        Logger.Initialize();
    }

    private void Start()
    {
        try
        {
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
    }

    private void Render()
    {
        Raylib.BeginDrawing();

        Raylib.EndDrawing();
    }

    private void Dispose()
    {
        
    }
}