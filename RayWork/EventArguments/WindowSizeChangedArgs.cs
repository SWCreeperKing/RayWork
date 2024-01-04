using System.Numerics;

namespace RayWork.EventArguments;

public class WindowSizeChangedEventArgs(Vector2 newWindowSize) : EventArgs
{
    public readonly Vector2 NewWindowSize = newWindowSize;
}