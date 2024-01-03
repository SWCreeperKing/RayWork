using Raylib_cs;

namespace RayWork.EventArguments;

public class KeyEvent : EventArgs
{
    public readonly KeyboardKey Key;

    public KeyEvent(KeyboardKey key) => Key = key;
}