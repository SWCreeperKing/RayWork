using Raylib_CsLo;

namespace RayWork.EventArguments;

public class KeyEvent : EventArgs
{
    public readonly KeyboardKey key;

    public KeyEvent(KeyboardKey key)
    {
        this.key = key;
    }
}