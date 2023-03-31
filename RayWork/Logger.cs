using System.Runtime.InteropServices;
using Raylib_CsLo;
using RayWork.EventArguments;
using static Raylib_CsLo.TraceLogLevel;
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

    private static readonly List<string> LogList = new();

    public static bool ShowDebugLogs = true;

    private static bool HasError;

    public static event EventHandler<LogReceivedEventArgs> LogRecieved;

    public static unsafe void Initialize()
    {
        Raylib.SetTraceLogCallback(&RayLog);

        LogRecieved += (_, args) =>
        {
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

            if (args.LogMessageLevel is Error)
            {
                Console.WriteLine($"[{args.TimeOfMessage}]\n\t{args.LogMessage}");
            }
            else
            {
                Console.WriteLine($"[{args.TimeOfMessage}]: [{args.LogMessage}]");
            }

            Console.ForegroundColor = ConsoleColor.White;
        };
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static unsafe void RayLog(int logLevel, sbyte* text, sbyte* args)
    {
        Log(logLevel switch
        {
            (int) LOG_ALL => Other,
            (int) LOG_TRACE or (int) LOG_DEBUG => Debug,
            (int) LOG_INFO or (int) LOG_NONE => Info,
            (int) LOG_WARNING => Warning,
            (int) LOG_ERROR or (int) LOG_FATAL => Error,
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
        }, $"from raylib: {LoggingLogger.Logging.GetLogMessage(new(text), new(args))} ");
    }

    public static void Log(string text) => Log(Debug, text);
    public static void Log(object text) => Log(Debug, text.ToString());
    public static void Log(Exception e) => Log(Error, $"{e.Message}\n{e.StackTrace}");

    public static T LogReturn<T>(T t)
    {
        Log(t.ToString());
        return t;
    }

    public static void Log(Level level, string text)
    {
        if (!ShowDebugLogs && level is Debug) return;

        var time = $"{DateTime.Now:G}";

        if (level is Error)
        {
            HasError = true;
            LogList.Add($"[{level}] [{time}]\n\t{text.Trim()}");
        }
        else
        {
            LogList.Add($"[{level}] [{time}] [{text.Trim()}]");
        }


        if (LogRecieved is not null)
        {
            LogRecieved(null, new(level, time, text.Trim()));
        }
    }

    public static void Log(Level level, object text) => Log(level, text.ToString());

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

    public static void CheckWrite()
    {
        if (HasError)
        {
            WriteLog();
        }
    }
}