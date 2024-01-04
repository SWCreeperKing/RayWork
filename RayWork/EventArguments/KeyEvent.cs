using Raylib_cs;

namespace RayWork.EventArguments;

public class KeyEvent(KeyboardKey key) : EventArgs
{
    public readonly KeyboardKey Key = key;
}