using Raylib_CsLo;
using RayWork.EventArguments;

namespace RayWork;

public static class Input
{
    // keyboard
    public static float KeyboardDelaySeconds { get; set; } = .5f;
    public static float KeyboardRepeatsPerSecond { get; set; } = 30;

    private static List<KeyboardKey> KeysActive = new();
    private static Dictionary<KeyboardKey, float> KeyDelay = new();
    private static Dictionary<KeyboardKey, float> KeyRepeatTimers = new();
    private static Dictionary<KeyboardKey, KeyEvent> KeyEventCache = new();

    public static event EventHandler<KeyEvent> OnKeyPressed;
    public static event EventHandler<KeyEvent> OnKeyReleased;
    public static event EventHandler<KeyEvent> OnKeyRepeat;

    // mouse
    public static GameObject MouseOccupier = null;

    public static MouseCursor CurrentMouseCursor
    {
        get => _CurrentMouseCursor;
        private set
        {
            _CurrentMouseCursor = value;
            Raylib.SetMouseCursor(value);
        }
    }

    private static MouseCursor _CurrentMouseCursor;

    private static List<MouseCursor> MouseCursorQueue = new();

    public static void UpdateInput(float deltaTime)
    {
        // update keyboard
        KeysActive.RemoveAll(key =>
        {
            if (Raylib.IsKeyDown(key)) return false;
            RemoveKey(key);
            return true;
        });

        if (OnKeyRepeat is not null)
        {
            foreach (var key in KeysActive)
            {
                if (KeyDelay[key] > 0)
                {
                    KeyDelay[key] -= deltaTime;
                    continue;
                }

                if (KeyRepeatTimers[key] > 0)
                {
                    KeyRepeatTimers[key] -= deltaTime;
                    continue;
                }

                OnKeyRepeat(null, GetKeyEvent(key));
                KeyRepeatTimers[key] += KeyboardRepeatsPerSecond / 1000f;
            }
        }

        var keyPressed = Raylib.GetKeyPressed_();
        while (keyPressed > 0)
        {
            AddKey(keyPressed);
            keyPressed = Raylib.GetKeyPressed_();
        }

        // update mouse
        if (MouseOccupier is not null)
        {
            CurrentMouseCursor = MouseOccupier.OccupiedMouseCursor();
        }
        else if (MouseCursorQueue.Any())
        {
            CurrentMouseCursor = MouseCursorQueue[^1];
        }
        else if (_CurrentMouseCursor is not MouseCursor.MOUSE_CURSOR_DEFAULT)
        {
            CurrentMouseCursor = MouseCursor.MOUSE_CURSOR_DEFAULT;
        }

        MouseCursorQueue.Clear();
    }

    public static bool IsKeyDown(KeyboardKey key) => KeysActive.Contains(key);
    public static bool IsKeyUp(KeyboardKey key) => !IsKeyDown(key);

    private static void RemoveKey(KeyboardKey key)
    {
        KeyDelay.Remove(key);
        KeyRepeatTimers.Remove(key);

        if (OnKeyReleased is null) return;
        
        OnKeyReleased(null, GetKeyEvent(key));
    }

    private static void AddKey(KeyboardKey key)
    {
        KeysActive.Add(key);
        KeyDelay.Add(key, KeyboardDelaySeconds);
        KeyRepeatTimers.Add(key, KeyboardRepeatsPerSecond / 1000f);

        if (OnKeyPressed is null) return;
        
        OnKeyPressed(null, GetKeyEvent(key));
    }

    private static KeyEvent GetKeyEvent(KeyboardKey key)
    {
        if (!KeyEventCache.ContainsKey(key))
        {
            KeyEventCache.Add(key, new(key));
        }

        return KeyEventCache[key];
    }

    public static void SetMouseCursor(MouseCursor mouseCursor) => MouseCursorQueue.Add(mouseCursor);
}