using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Raylib_cs;
using RayWork.EventArguments;
using static Raylib_cs.TraceLogLevel;
using static RayWork.Logger.Level;

namespace RayWork;

public static class Logger
{
    public enum Level
    {
        Info,
        Debug,
        Special,
        Warning,
        SoftError,
        Error,
        Other
    }

    public static readonly string CrashSave = $"{Directory.GetCurrentDirectory().Replace('\\', '/')}/CrashLogs";
    public static readonly string StatusSave = $"{Directory.GetCurrentDirectory().Replace('\\', '/')}/CrashLogs";
    public static readonly string Guid = System.Guid.NewGuid().ToString();

    private static readonly List<string> LogList = [];

    public static bool ShowDebugLogs = true;

    private static bool HasError;

    public static event EventHandler<LogReceivedEventArgs>? LogReceived;

    public static unsafe void Initialize()
    {
        Raylib.SetTraceLogCallback(&RayLog);

        LogReceived += GameConsole.LogMessage;
        LogReceived += (_, args) =>
        {
            if (!RayApplication.PrintLogToWindowsConsole) return;
            Console.ForegroundColor = args.LogMessageLevel switch
            {
                Info => ConsoleColor.DarkGreen,
                Debug => ConsoleColor.DarkCyan,
                Warning => ConsoleColor.Yellow,
                Error or SoftError => ConsoleColor.Red,
                Other => ConsoleColor.Blue,
                Special => ConsoleColor.Cyan,
                _ => throw new ArgumentOutOfRangeException(nameof(args.LogMessageLevel), args.LogMessageLevel, null)
            };

            Console.WriteLine(args);
            Console.ForegroundColor = ConsoleColor.White;
        };
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe void RayLog(int logLevel, sbyte* text, sbyte* args)
    {
        Log(logLevel switch
            {
                (int) LOG_ALL => Other,
                (int) LOG_TRACE or (int) LOG_DEBUG => Debug,
                (int) LOG_INFO or (int) LOG_NONE => Info,
                (int) LOG_WARNING => Warning,
                (int) LOG_ERROR or (int) LOG_FATAL => Error,
                _ => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
            }, $"{Logging.GetLogMessage(new IntPtr(text), new IntPtr(args))}", "raylib");
    }

    public static void Log(string? text, string sender = "app") => Log(Debug, text!, sender);
    public static void Log(object text, string sender = "app") => Log(Debug, text.ToString()!, sender);
    public static void Log(Exception e, string sender = "app") => Log(Error, $"{e.Message}\n{e.StackTrace}", sender);

    public static T LogReturn<T>(T t)
    {
        Log(t!.ToString()!);
        return t;
    }

    public static void Log(Level level, string text, string sender)
    {
        if (!ShowDebugLogs && level is Debug) return;
        var time = $"{DateTime.Now:G}";
        LogList.Add(Format(level, time, sender, text));
        LogReceived?.Invoke(null, new LogReceivedEventArgs(level, time, text.Trim(), sender));
    }

    public static void Log(Level level, object? text, string sender = "app") => Log(level, text!.ToString()!, sender);

    public static void WriteLog(bool isCrash = true)
    {
        HasError = false;
        var dir = isCrash ? "CrashLogs" : "StatusLogs";
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        var file = isCrash
            ? $"CrashLogs/Crash {DateTime.Now:u}.log".Replace(' ', '_').Replace(':', '-')
            : $"StatusLogs/Status {Guid}.log";

        using var sw = File.CreateText(file);
        sw.Write(string.Join("\n", LogList));
        sw.Close();

        Console.WriteLine(
            $"SAVED {(isCrash ? "CRASH" : "STATUS")} LOG AT: {Directory.GetCurrentDirectory().Replace('\\', '/')}/{file}");
    }

    public static string Format(LogReceivedEventArgs args)
        => Format(args.LogMessageLevel, args.TimeOfMessage, args.Sender, args.LogMessage);

    public static string Format(Level level, string time, string sender, string text)
    {
        if (level is Error)
        {
            HasError = true;
            return $"[{time}]  [{level}] From [{sender}]  Sent Error:\n\t{text.Trim()}";
        }

        return $"[{time}]  [{level}] From [{sender}]  [{text.Trim()}]";
    }

    public static void CheckWrite()
    {
        if (!HasError) return;
        WriteLog();
    }
}