using System.Numerics;
using Raylib_CsLo;
using RayWork;
using RayWork.Objects;
using RayWork.SaveSystem;

namespace RayWorkTester;

public class SimonSays : Scene
{
    private static readonly Random Random = new();

    public SimonButton[] Buttons;
    public List<int> Order = new();

    public bool ReadIn;
    public float ReadWaitSeconds = 1;
    public HighScore HighScore = new();

    private int ReadIndex;
    private int ReadInput;

    public override void Initialize()
    {
        SaveSystem.InitializeSaveSystem("SW_CreeperKing", "SimonSays");
        SaveSystem.AddSaveItem("highscore", HighScore);
        SaveSystem.LoadItems();

        Buttons = new SimonButton[]
        {
            new(new(300), new(75), Raylib.BLUE.MakeDarker()),
            new(new(300, 390), new(75), Raylib.GREEN.MakeDarker()),
            new(new(390, 300), new(75), Raylib.RED.MakeDarker()),
            new(new(390), new(75), Raylib.YELLOW.MakeDarker())
        };

        for (var i = 0; i < Buttons.Length; i++)
        {
            var button = Buttons[i];
            var i1 = i;
            button.ButtonComponent.OnClicked += (_, _) => Pressed(i1);
            AddChild(button);
        }

        Button resetButton = new("Restart", new(500));
        resetButton.OnButtonPressed += (_, _) => Reset();

        AddChild(resetButton);
        AddToOrder();
    }

    public override void UpdateLoop()
    {
        if (ReadWaitSeconds > 0)
        {
            ReadWaitSeconds -= RayApplication.DeltaTime;
            return;
        }

        if (ReadIn) return;
        if (!Buttons[Order[ReadIndex]].Active)
        {
            Buttons[Order[ReadIndex]].Active = true;
        }

        if (!Buttons[Order[ReadIndex]].ShowOrder()) return;
        ReadIndex++;

        if (ReadIndex < Order.Count) return;
        ReadIndex = 0;
        ReadIn = true;
    }

    public override void RenderLoop()
        => Raylib.DrawText($"Score: {Order.Count}\nHigh Score: {HighScore.Score}", 15, 15, 24, Raylib.BLUE);

    public void ResetButtons()
    {
        foreach (var button in Buttons)
        {
            button.Reset();
        }
    }

    public void Reset()
    {
        ResetButtons();
        ReadIndex = 0;
        ReadInput = 0;
        ReadIn = false;
        ReadWaitSeconds = 1f;
    }

    public void AddToOrder() => Order.Add(Random.Next(Buttons.Length));

    public void Pressed(int i)
    {
        if (!ReadIn) return;
        if (i != Order[ReadInput])
        {
            Reset();
            OnFail();
            return;
        }

        ReadInput++;
        if (ReadInput < Order.Count) return;

        ReadInput = 0;
        AddToOrder();
        Reset();
    }

    public void OnFail()
    {
        HighScore.Score = Math.Max(Order.Count, HighScore.Score);
        SaveSystem.OpenDirectory();
        Order.Clear();
        AddToOrder();
    }

    public override void DisposeLoop() => SaveSystem.SaveItems();
}

public class HighScore
{
    public int Score;
}