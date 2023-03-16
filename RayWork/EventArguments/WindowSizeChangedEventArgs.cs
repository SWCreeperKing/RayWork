using System.Numerics;

namespace RayWork.EventArguments;

public class WindowSizeChangedEventArgs : EventArgs
{
    public readonly Vector2 newWindowSize;

    public WindowSizeChangedEventArgs(Vector2 newWindowSize)
    {
        this.newWindowSize = newWindowSize;
    }
}