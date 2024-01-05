using System.Numerics;
using ImGuiNET;
using Raylib_cs;
using RayWork;
using RayWork.ComponentObjects;
using RayWork.Objects;
using RayWork.SaveSystem;
using static Raylib_cs.Color;

namespace RayWorkTester.SimonSays;

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
    private string OrderString = "N/A";

    public override string Label => "simon";

    public override void Initialize()
    {
        SaveSystem.InitializeSaveSystem("SW_CreeperKing", "SimonSays");
        SaveSystem.AddSaveItem("highscore", HighScore);
        SaveSystem.LoadItems();

        Buttons =
        [
            new SimonButton(new Vector2(300), new Vector2(75), BLUE.MakeDarker()),
            new SimonButton(new Vector2(300, 390), new Vector2(75), GREEN.MakeDarker()),
            new SimonButton(new Vector2(390, 300), new Vector2(75), RED.MakeDarker()),
            new SimonButton(new Vector2(390), new Vector2(75), YELLOW.MakeDarker())
        ];

        for (var i = 0; i < Buttons.Length; i++)
        {
            var button = Buttons[i];
            var i1 = i;
            button.OnClicked += (_, _) => Pressed(i1);
            AddChild(button);
        }

        Button resetButton = new("Restart", new Vector2(500));
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
        => Raylib.DrawText($"Score: {Order.Count}\nHigh Score: {HighScore.Score}", 15, 15, 24, BLUE);

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

    public void AddToOrder()
    {
        Order.Add(Random.Next(Buttons.Length));
        OrderString = string.Join(", ", Order);
    }
    
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

    public override void DebugLoop()
    {
        ImGui.Text(OrderString);

        if (!ImGui.Button("CREATE NEW ORDER")) return;
        Reset();
        Order.Clear();
        AddToOrder();
    }

    public override void Dispose() => SaveSystem.SaveItems();
}

public class HighScore
{
    public int Score;
}