using Raylib_cs;
using RayWork.EventArguments;

namespace RayWork;

public static class Input
{
    public static readonly MouseButton[] MouseButtons = Enum.GetValues<MouseButton>();

    // keyboard
    public static bool HandleKeyboardEvents = true;
    public static float KeyboardDelaySeconds { get; set; } = .5f;
    public static float KeyboardRepeatsPerSecond { get; set; } = 30;

    private static List<KeyboardKey> KeysActive = [];
    private static Dictionary<KeyboardKey, float> KeyDelay = new();
    private static Dictionary<KeyboardKey, float> KeyRepeatTimers = new();
    private static Dictionary<KeyboardKey, KeyEvent> KeyEventCache = new();

    public static event EventHandler<KeyEvent> OnKeyPressed;
    public static event EventHandler<KeyEvent> OnKeyReleased;
    public static event EventHandler<KeyEvent> OnKeyRepeat;

    // mouse
    public static bool HandleMouseEvents = true;
    public static GameObject? MouseOccupier = null;

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
    private static List<MouseCursor> MouseCursorQueue = [];

    public static event EventHandler<MouseStateEvent> MouseEvent;

    public static void UpdateInput(float deltaTime)
    {
        // update keyboard
        if (HandleKeyboardEvents)
        {
            HandleKeyboardEvent(deltaTime);
        }

        // update mouse
        if (HandleMouseEvents)
        {
            HandleMouseEvent();
        }
    }

    private static void HandleKeyboardEvent(float deltaTime)
    {
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

        var keyPressed = Raylib.GetKeyPressed();
        while (keyPressed > 0)
        {
            AddKey((KeyboardKey) keyPressed);
            keyPressed = Raylib.GetKeyPressed();
        }
    }

    private static void HandleMouseEvent()
    {
        if (MouseEvent is not null)
        {
            var mousePosition = Raylib.GetMousePosition();
            var mousePressed = MouseButtons.Where(button => Raylib.IsMouseButtonPressed(button));
            var mouseDown = MouseButtons.Where(button => Raylib.IsMouseButtonDown(button));
            MouseStateEvent mouseState = new(mousePosition, mousePressed, mouseDown);

            MouseEvent(null, mouseState);
        }

        if (MouseOccupier is not null)
        {
            CurrentMouseCursor = MouseOccupier.OccupiedMouseCursor();
        }
        else if (MouseCursorQueue.Count != 0)
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
        if (KeysActive.Contains(key))
        {
            KeyDelay.Remove(key);
            KeyRepeatTimers.Remove(key);
        }

        if (OnKeyReleased is null) return;

        OnKeyReleased(null, GetKeyEvent(key));
    }

    private static void AddKey(KeyboardKey key)
    {
        if (!KeysActive.Contains(key))
        {
            KeysActive.Add(key);
            KeyDelay.Add(key, KeyboardDelaySeconds);
            KeyRepeatTimers.Add(key, KeyboardRepeatsPerSecond / 1000f);
        }
        else
        {
            KeyRepeatTimers[key] = KeyboardRepeatsPerSecond / 1000f;
        }

        if (OnKeyPressed is null) return;

        OnKeyPressed(null, GetKeyEvent(key));
    }

    private static KeyEvent GetKeyEvent(KeyboardKey key)
    {
        if (!KeyEventCache.TryGetValue(key, out var keyEvent))
        {
            keyEvent = KeyEventCache[key] = new KeyEvent(key);
        }

        return keyEvent;
    }

    public static void SetMouseCursor(MouseCursor mouseCursor) => MouseCursorQueue.Add(mouseCursor);
}