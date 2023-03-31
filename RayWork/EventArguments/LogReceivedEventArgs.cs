using System.Runtime.InteropServices;
using static RayWork.EventArguments.LoggingLogger.Native;

namespace RayWork.EventArguments;

public class LogReceivedEventArgs : EventArgs
{
    public readonly Logger.Level LogMessageLevel;
    public readonly string TimeOfMessage;
    public readonly string LogMessage;

    public LogReceivedEventArgs(Logger.Level logMessageLevel, string timeOfMessage, string logMessage)
    {
        LogMessageLevel = logMessageLevel;
        TimeOfMessage = timeOfMessage;
        LogMessage = logMessage;
    }
}

/// <summary>
/// Stolen from Raylib-cs since Raylib-cslo didn't have it
/// </summary>
internal static class LoggingLogger
{
    internal readonly struct Native
    {
        private const string Msvcrt = "msvcrt";
        private const string Libc = "libc";
        private const string LibSystem = "libSystem";

        [DllImport(LibSystem, EntryPoint = "vasprintf", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vasprintf_apple(ref nint buffer, nint format, nint args);

        [DllImport(Libc, EntryPoint = "vsprintf", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vsprintf_linux(nint buffer, nint format, nint args);

        [DllImport(Msvcrt, EntryPoint = "vsprintf", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vsprintf_windows(nint buffer, nint format, nint args);

        [DllImport(Libc, EntryPoint = "vsnprintf", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vsnprintf_linux(nint buffer, nuint size, nint format, nint args);

        [DllImport(Msvcrt, EntryPoint = "vsnprintf", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vsnprintf_windows(nint buffer, nuint size, nint format, nint args);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    private struct VaListLinuxX64
    {
        uint gpOffset;
        uint fpOffset;
        nint overflowArgArea;
        nint regSaveArea;
    }

    /// <summary>
    /// Logging workaround for formatting strings from native code
    ///
    /// and maybe reformatted a bit :)
    /// </summary>
    public static unsafe class Logging
    {
        public static string GetLogMessage(nint format, nint args)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return AppleLogCallback(format, args);
            }

            // Special marshalling is needed on Linux desktop 64 bits.
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && nint.Size == 8)
            {
                return LinuxX64LogCallback(format, args);
            }

            var byteLength = Vsnprintf(nint.Zero, nuint.Zero, format, args) + 1;
            if (byteLength <= 1)
            {
                return string.Empty;
            }

            var buffer = Marshal.AllocHGlobal(byteLength);
            Vsprintf(buffer, format, args);

            var result = Marshal.PtrToStringUTF8(buffer);
            Marshal.FreeHGlobal(buffer);

            return result;
        }

        private static string AppleLogCallback(nint format, nint args)
        {
            var buffer = nint.Zero;
            try
            {
                var count = vasprintf_apple(ref buffer, format, args);
                if (count == -1)
                {
                    return string.Empty;
                }

                return Marshal.PtrToStringUTF8(buffer) ?? string.Empty;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        private static string LinuxX64LogCallback(nint format, nint args)
        {
            // The args pointer cannot be reused between two calls. We need to make a copy of the underlying structure.
            var listStructure = Marshal.PtrToStructure<VaListLinuxX64>(args);
            var listPointer = nint.Zero;
            int byteLength;
            string result;

            // Get length of args
            listPointer = Marshal.AllocHGlobal(Marshal.SizeOf(listStructure));
            Marshal.StructureToPtr(listStructure, listPointer, false);
            byteLength = vsnprintf_linux(nint.Zero, nuint.Zero, format, listPointer) + 1;

            // Allocate buffer for result
            Marshal.StructureToPtr(listStructure, listPointer, false);

            var utf8Buffer = nint.Zero;
            utf8Buffer = Marshal.AllocHGlobal(byteLength);

            // Print result into buffer
#pragma warning disable CA1806
            vsprintf_linux(utf8Buffer, format, listPointer);
#pragma warning restore CA1806
            result = Marshal.PtrToStringUTF8(utf8Buffer);

            Marshal.FreeHGlobal(listPointer);
            Marshal.FreeHGlobal(utf8Buffer);

            return result;
        }

        // https://github.com/dotnet/runtime/issues/51052
        static int Vsnprintf(nint buffer, nuint size, nint format, nint args)
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? vsnprintf_windows(buffer, size, format, args)
                : RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                    ? vsnprintf_linux(buffer, size, format, args)
                    : RuntimeInformation.IsOSPlatform(OSPlatform.Create("ANDROID"))
                        ? vsprintf_linux(buffer, format, args)
                        : -1;
        }

        // https://github.com/dotnet/runtime/issues/51052
        static int Vsprintf(nint buffer, nint format, nint args)
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? vsprintf_windows(buffer, format, args)
                : RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                    ? vsprintf_linux(buffer, format, args)
                    : RuntimeInformation.IsOSPlatform(OSPlatform.Create("ANDROID"))
                        ? vsprintf_linux(buffer, format, args)
                        : -1;
        }
    }
}