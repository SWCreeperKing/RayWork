using System.Numerics;
using Raylib_CsLo;
using RayWork;
using RayWork.Objects;
using RayWork.SaveSystem;

namespace RayWorkTester;

public class SimonSays : Scene
{
    private static readonly Random Random = new();

    public SimonButton[] buttons;
    public List<int> order = new();

    public bool readIn;
    public float readWaitSeconds = 1;
    public HighScore highScore = new();

    private int _readIndex = 0;
    private int _readInput = 0;

    public override void Initialize()
    {
        SaveSystem.InitializeSaveSystem("SW_CreeperKing", "SimonSays");
        SaveSystem.AddSaveItem("highscore", highScore);
        SaveSystem.LoadItems();

        buttons = new SimonButton[]
        {
            new(new Vector2(300), new Vector2(75), Raylib.BLUE.MakeDarker()),
            new(new Vector2(300, 390), new Vector2(75), Raylib.GREEN.MakeDarker()),
            new(new Vector2(390, 300), new Vector2(75), Raylib.RED.MakeDarker()),
            new(new Vector2(390), new Vector2(75), Raylib.YELLOW.MakeDarker())
        };
        
        for (var i = 0; i < buttons.Length; i++)
        {
            var button = buttons[i];
            var i1 = i;
            button.buttonComponent.OnClicked += (_, _) => Pressed(i1);
            AddChild(button);
        }

        Button resetButton = new("Restart", new Vector2(500));
        resetButton.OnButtonPressed += (_, _) => Reset();

        AddChild(resetButton);
        
        AddToOrder();
    }

    public override void UpdateLoop()
    {
        if (readWaitSeconds > 0)
        {
            readWaitSeconds -= RayApplication.DeltaTime;
            return;
        }

        if (readIn) return;
        if (!buttons[order[_readIndex]].active)
        {
            buttons[order[_readIndex]].active = true;
        }

        if (!buttons[order[_readIndex]].ShowOrder()) return;
        _readIndex++;
        
        if (_readIndex < order.Count) return;
        _readIndex = 0;
        readIn = true;
    }

    public override void RenderLoop()
    {
        Raylib.DrawText($"Score: {order.Count}\nHigh Score: {highScore.highScore}", 15, 15, 24, Raylib.BLUE);
    }

    public void ResetButtons()
    {
        foreach (var button in buttons)
        {
            button.Reset();
        }
    }

    public void Reset()
    {
        ResetButtons();
        _readIndex = 0;
        _readInput = 0;
        readIn = false;
        readWaitSeconds = 1f;
    }
    
    public void AddToOrder()
    {
        order.Add(Random.Next(buttons.Length));
    }

    public void Pressed(int i)
    {
        if (!readIn) return;
        if (i != order[_readInput])
        {
            Reset();
            OnFail();
            return;
        }
        
        _readInput++;
        if (_readInput < order.Count) return;

        _readInput = 0;
        AddToOrder();
        Reset();
    }

    public void OnFail()
    {
        highScore.highScore = Math.Max(order.Count, highScore.highScore);
        SaveSystem.OpenDirectory();
        order.Clear();
        AddToOrder();
    }

    public override void DisposeLoop()
    {
        SaveSystem.SaveItems();
    }
}

public class HighScore
{
    public int highScore;
}