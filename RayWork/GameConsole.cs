using ImGuiNET;
using RayWork.Commands;
using RayWork.EventArguments;
using RayWork.RLImgui;
using static ImGuiNET.ImGuiInputTextFlags;
using static Raylib_cs.Color;
using static RayWork.Logger;
using static RayWork.Logger.Level;

namespace RayWork;

public static class GameConsole
{
    public static bool IsConsoleOpen;

    public static readonly Dictionary<Level, uint> Colors = new()
    {
        [Info] = GREEN.MakeDarker().ToUint(),
        [Debug] = ORANGE.ToUint(),
        [Warning] = YELLOW.ToUint(),
        [SoftError] = RED.ToUint(),
        [Error] = RED.ToUint(),
        [Other] = BLUE.ToUint(),
        [Special] = SKYBLUE.ToUint()
    };

    public static int MaxScrollback = 200;
    public static bool ScrollToBottom = true;

    private static Queue<LogMessage> Scrollback = [];
    private static string Input = "";
    private static bool ToScroll;

    public static void Render()
    {
        if (!IsConsoleOpen) return;
        if (!ImGui.Begin("Console")) return;
        var wSize = ImGui.GetContentRegionMax();
        ImGui.BeginChild("console.child", wSize with { Y = wSize.Y - 50 }, ImGuiChildFlags.Border);

        foreach (var message in Scrollback)
        {
            ImGui.PushStyleColor(ImGuiCol.Text, Colors[message.Level]);
            ImGui.Text(message.Message);
            ImGui.PopStyleColor();
        }

        if (ScrollToBottom && ToScroll)
        {
            ImGui.SetScrollHereY();
            ToScroll = false;
        }
        else if (ToScroll)
        {
            ToScroll = false;
        }

        ImGui.EndChild();

        if (!ImGui.InputTextWithHint("", "Command", ref Input, 999, EnterReturnsTrue)) return;
        CommandRegister.RunCommand(Input);
        UpdateScrollBack();
        Input = "";
    }

    public static void LogMessage(object? sender, LogReceivedEventArgs args)
        => Scrollback.Enqueue(new LogMessage(args));

    public static void LogMessage(LogMessage message)
    {
        Scrollback.Enqueue(message);
        UpdateScrollBack();
    }

    private static void UpdateScrollBack()
    {
        while (Scrollback.Count > MaxScrollback) Scrollback.Dequeue();
        if (ScrollToBottom) ToScroll = true;
    }

    public static void ToggleConsole() => IsConsoleOpen = !IsConsoleOpen;
}

public readonly struct LogMessage(LogReceivedEventArgs args)
{
    public readonly Level Level = args.LogMessageLevel;
    public readonly string Message = Format(args);
}