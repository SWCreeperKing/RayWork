using Raylib_CsLo;
using RayWork.EventArguments;

namespace RayWork;

public static class Input
{
    public static float KeyboardDelaySeconds { get; set; } = .5f;
    public static float KeyboardRepeatsPerSecond { get; set; } = 30;

    private static List<KeyboardKey> _keysActive = new();
    private static Dictionary<KeyboardKey, float> _keyDelay = new();
    private static Dictionary<KeyboardKey, float> _keyRepeatTimers = new();
    private static Dictionary<KeyboardKey, KeyEvent> _keyEventCache = new();

    public static event EventHandler<KeyEvent> OnKeyPressed;
    public static event EventHandler<KeyEvent> OnKeyReleased;
    public static event EventHandler<KeyEvent> OnKeyRepeat;

    public static bool IsKeyDown(KeyboardKey key)
    {
        return _keysActive.Contains(key);
    }

    public static bool IsKeyUp(KeyboardKey key)
    {
        return !IsKeyDown(key);
    }

    public static void UpdateKeys(float deltaTime)
    {
        _keysActive.RemoveAll(key =>
        {
            if (Raylib.IsKeyDown(key)) return false;
            RemoveKey(key);
            return true;
        });

        if (OnKeyRepeat is not null)
        {
            foreach (var key in _keysActive)
            {
                if (_keyDelay[key] > 0)
                {
                    _keyDelay[key] -= deltaTime;
                    continue;
                }
                
                if (_keyRepeatTimers[key] > 0)
                {
                    _keyRepeatTimers[key] -= deltaTime;
                    continue;
                }
                
                OnKeyRepeat(null, GetKeyEvent(key));
                _keyRepeatTimers[key] += KeyboardRepeatsPerSecond / 1000f;
            }
        }

        var keyPressed = Raylib.GetKeyPressed_();
        while (keyPressed > 0)
        {
            AddKey(keyPressed);
            keyPressed = Raylib.GetKeyPressed_();
        }
    }

    private static void RemoveKey(KeyboardKey key)
    {
        _keyDelay.Remove(key);
        _keyRepeatTimers.Remove(key);

        if (OnKeyReleased is null) return;
        OnKeyReleased(null, GetKeyEvent(key));
    }

    private static void AddKey(KeyboardKey key)
    {
        _keysActive.Add(key);
        _keyDelay.Add(key, KeyboardDelaySeconds);
        _keyRepeatTimers.Add(key, KeyboardRepeatsPerSecond / 1000f);

        if (OnKeyPressed is null) return;
        OnKeyPressed(null, GetKeyEvent(key));
    }

    private static KeyEvent GetKeyEvent(KeyboardKey key)
    {
        if (!_keyEventCache.ContainsKey(key)) _keyEventCache.Add(key, new KeyEvent(key));
        return _keyEventCache[key];
    }
}