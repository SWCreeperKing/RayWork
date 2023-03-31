using System.Numerics;

namespace RayWork.EventArguments;

public class WindowSizeChangedEventArgs : EventArgs
{
    public readonly Vector2 NewWindowSize;

    public WindowSizeChangedEventArgs(Vector2 newWindowSize) => NewWindowSize = newWindowSize;
}