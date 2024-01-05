using System.Diagnostics;
using System.Globalization;
using System.Text;
using Raylib_cs;
using static RayWork.Logger.Level;
using static RayWork.SaveSystem.SaveSystem;

namespace RayWork.Commands;

public static class DefaultCommands
{
    [Command("help"), Help("Provides information on all commands")]
    public static LogReturn Help(string[] args)
    {
        if (args.Length > 0)
            return !CommandRegister.TryGetHelp(args[0], out var specificHelp)
                ? new LogReturn($"Command [{args[0]}] Does Not Exist Or Does Not Have Help", SoftError)
                : new LogReturn($":\n{FormatHelp(args[0], specificHelp!)}\n:");

        StringBuilder sb = new();
        sb.Append(":\n");
        foreach (var (command, help) in CommandRegister.GetHelp())
        {
            sb.Append(FormatHelp(command, help)).Append('\n');
        }

        sb.Append(':');

        return new LogReturn(sb.ToString());

        string FormatHelp(string command, string help)
            => $"""
                - {command}
                    -> {help.Replace("\n", "\n    -> ")}
                """;
    }

    [Command("setfps"), Help("setfps [n > 0]\nsets fps to n")]
    public static LogReturn SetFps(string[] args)
    {
        if (args.Length < 1) return new LogReturn("Not Enough Arguments", SoftError);
        if (!int.TryParse(args[0], NumberStyles.Any, CultureInfo.InvariantCulture, out var fpsset) || fpsset <= 0)
            return new LogReturn($"[{args[0]}] <- IS NOT A VALID NUMBER", SoftError);
        Raylib.SetTargetFPS(fpsset);
        return new LogReturn($"Fps set to [{fpsset}]");
    }

    [Command("opensavedir"), Help("opens save directory")]
    public static LogReturn OpenSaveDirectory(string[] args)
    {
        if (!SaveSystemInitialized) return new LogReturn("Save system is not initialized", SoftError);
        Process.Start("explorer.exe", $"{SaveDirectory}".Replace("/", "\\"));
        return new LogReturn($"Opening to [{SaveDirectory}]");
    }
}