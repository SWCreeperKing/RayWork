using Raylib_cs;
using RayWork.EventArguments;
using RayWork.Objects;

namespace RayWork;

public static class Input
{
    public static readonly MouseButton[] MouseButtons = Enum.GetValues<MouseButton>();

    // keyboard
    public static bool HandleKeyboardEvents = true;
    public static float KeyboardDelaySeconds { get; set; } = .5f;
    public static float KeyboardRepeatsPerSecond { get; set; } = 30;

    private static List<KeyboardKey> KeysActive = [];
    private static HashSet<KeyboardKey> KeysReleased = [];
    private static HashSet<KeyboardKey> KeysPressed = [];
    private static Dictionary<KeyboardKey, float> KeyDelay = new();
    private static Dictionary<KeyboardKey, float> KeyRepeatTimers = new();
    private static Dictionary<KeyboardKey, KeyEvent> KeyEventCache = new();

    public static event EventHandler<KeyEvent>? OnKeyPressed;
    public static event EventHandler<KeyEvent>? OnKeyReleased;
    public static event EventHandler<KeyEvent>? OnKeyRepeat;

    // mouse
    public static bool HandleMouseEvents = true;
    public static GameObject? MouseOccupier = null;
    public static MouseState CurrentMouseState { get; private set; }

    public static MouseCursor CurrentMouseCursor
    {
        get => CurrentMouseCursorHolder;
        private set
        {
            CurrentMouseCursorHolder = value;
            Raylib.SetMouseCursor(value);
        }
    }

    private static MouseCursor CurrentMouseCursorHolder;
    private static List<MouseCursor> MouseCursorQueue = [];

    public static event EventHandler<MouseStateEvent>? MouseEvent;

    public static void UpdateInput(float deltaTime)
    {
        // update keyboard
        if (HandleKeyboardEvents)
        {
            KeysPressed = [];
            KeysReleased = [];
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

        foreach (var key in RLImgui.RlImgui.Keys) AddKey(key);
    }

    private static void HandleMouseEvent()
    {
        var mousePosition = Raylib.GetMousePosition();
        var mousePressed = MouseButtons.Where(button => Raylib.IsMouseButtonPressed(button));
        var mouseDown = MouseButtons.Where(button => Raylib.IsMouseButtonDown(button));
        var windowSize = RayApplication.WindowSize;
        var quad = mousePosition.X > windowSize.X / 2 ? 1 : 2;
        if (mousePosition.Y > windowSize.Y / 2) quad += 2;
        CurrentMouseState =
            new MouseState(mousePosition, (ScreenQuadrant) quad, mousePressed, mouseDown);

        MouseEvent?.Invoke(null, new MouseStateEvent(CurrentMouseState));

        if (MouseOccupier is not null)
        {
            CurrentMouseCursor = MouseOccupier.OccupiedMouseCursor();
        }
        else if (MouseCursorQueue.Count != 0)
        {
            CurrentMouseCursor = MouseCursorQueue[^1];
        }
        else if (CurrentMouseCursorHolder is not MouseCursor.MOUSE_CURSOR_DEFAULT)
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

        KeysReleased.Add(key);
        OnKeyReleased?.Invoke(null, GetKeyEvent(key));
    }

    private static void AddKey(KeyboardKey key)
    {
        if (KeysActive.Contains(key))
        {
            KeyRepeatTimers[key] = KeyboardRepeatsPerSecond / 1000f;
        }
        else
        {
            KeysActive.Add(key);
            KeyDelay.Add(key, KeyboardDelaySeconds);
            KeyRepeatTimers.Add(key, KeyboardRepeatsPerSecond / 1000f);
        }

        KeysPressed.Add(key);
        OnKeyPressed?.Invoke(null, GetKeyEvent(key));
    }

    private static KeyEvent GetKeyEvent(KeyboardKey key)
    {
        if (!KeyEventCache.TryGetValue(key, out var keyEvent)) return KeyEventCache[key] = new KeyEvent(key);
        return keyEvent;
    }

    public static bool IsKeyPressed(KeyboardKey key) => KeysPressed.Contains(key);
    public static bool IsKeyReleased(KeyboardKey key) => KeysReleased.Contains(key);
    public static void SetMouseCursor(MouseCursor mouseCursor) => MouseCursorQueue.Add(mouseCursor);
}