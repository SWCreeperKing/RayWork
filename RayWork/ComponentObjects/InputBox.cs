using System.Numerics;
using Raylib_CsLo;
using RayWork.CoreComponents;
using static Raylib_CsLo.KeyboardKey;

namespace RayWork.Objects;

public class InputBox : GameObject
{
    public static readonly string[] CapitalNumbers = { ")", "!", "@", "#", "$", "%", "^", "&", "*", "(" };

    public static readonly Dictionary<KeyboardKey, string> KeyCharacters = new()
    {
        [KEY_COMMA] = ",", [KEY_SPACE] = " ", [KEY_TAB] = "\t", [KEY_PERIOD] = ".",
        [KEY_SLASH] = "/", [KEY_BACKSLASH] = "\\", [KEY_MINUS] = "-", [KEY_EQUAL] = "=",
        [KEY_LEFT_BRACKET] = "[", [KEY_RIGHT_BRACKET] = "]", [KEY_SEMICOLON] = ";", [KEY_APOSTROPHE] = "'",
        [KEY_GRAVE] = "`"
    };

    public static readonly Dictionary<KeyboardKey, string> CapitalKeyCharacters = new()
    {
        [KEY_COMMA] = "<", [KEY_PERIOD] = ">", [KEY_SLASH] = "?", [KEY_BACKSLASH] = "|", [KEY_MINUS] = "_",
        [KEY_EQUAL] = "+", [KEY_LEFT_BRACKET] = "{", [KEY_RIGHT_BRACKET] = "}", [KEY_SEMICOLON] = ":",
        [KEY_APOSTROPHE] = "\"", [KEY_GRAVE] = "~"
    };

    public RectangleComponent rectangleComponent;
    public PanelComponent panelComponent;
    public TextComponent textComponent;

    public Vector2 padding
    {
        get => _padding;
        set
        {
            _padding = value;
            _padding2 = value * 2;
        }
    }

    public string text = "";
    public string cursorChar = "_";
    public string cursorBlinkChar = " ";
    public int cursorPosition;
    public bool doCursorBlinking = true;
    public float cursorBlinkCooldownSeconds = .75f;
    public float cursorBlinkDurationSeconds = .25f;

    private Vector2 _padding = new(3);
    private Vector2 _padding2 = new(6);
    private bool cursorBlink = false;
    private float cursorTimer;

    public InputBox(TransformComponent transformComponent, SizeComponent sizeComponent)
    {
        AddComponent(textComponent = new(text));
        AddComponent(panelComponent =
            new(rectangleComponent = new RectangleComponent(transformComponent, sizeComponent)));
        SetupInputEvent();
    }

    public InputBox(Vector2 position, Vector2 size)
    {
        AddComponent(textComponent = new(text));
        AddComponent(panelComponent =
            new(rectangleComponent =
                new((PositionComponent) position, (StaticSizeComponent) size)));
        SetupInputEvent();
    }

    public override void UpdateLoop()
    {
        if (Input.MouseOccupier != this)
        {
            if (Input.MouseOccupier is not null ||
                !panelComponent.rectangleComponent.Rectangle.IsMouseIn() ||
                !Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            {
                return;
            }

            cursorBlink = false;
            cursorTimer = cursorBlinkCooldownSeconds;
            Input.MouseOccupier = this;
        }

        if (panelComponent.rectangleComponent.Rectangle.IsMouseIn() ||
            !Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
        {
            if (doCursorBlinking)
            {
                cursorTimer -= RayApplication.DeltaTime;
                if (cursorTimer <= 0)
                {
                    cursorBlink = !cursorBlink;
                    cursorTimer = cursorBlink ? cursorBlinkDurationSeconds : cursorBlinkCooldownSeconds;
                }

                textComponent.text = text.Insert(cursorPosition, cursorBlink ? cursorBlinkChar : cursorChar);
            } else textComponent.text = text.Insert(cursorPosition, cursorChar);

            return;
        }

        FinishTyping();
    }

    public override void RenderLoop()
    {
        panelComponent.DrawPanel();
        rectangleComponent.Rectangle.DrawMask(rectangleComponent.Size - _padding2, () =>
            textComponent.DrawText(rectangleComponent.Position + _padding, Vector2.Zero));
    }

    public override MouseCursor OccupiedMouseCursor()
    {
        return MouseCursor.MOUSE_CURSOR_IBEAM;
    }

    private void SetupInputEvent()
    {
        Input.OnKeyPressed += (_, key) => KeyEvent(key.key);
        Input.OnKeyRepeat += (_, key) => KeyEvent(key.key);
    }

    private void KeyEvent(KeyboardKey key)
    {
        if (Input.MouseOccupier != this) return;
        var shift = Raylib.IsKeyDown(KEY_LEFT_SHIFT) || Raylib.IsKeyDown(KEY_RIGHT_SHIFT);

        switch (key)
        {
            case >= KEY_A and <= KEY_Z:
                var character = (int) key;
                if (!shift) character += 32;
                text = text.Insert(cursorPosition, $"{(char) character}");
                cursorPosition++;
                break;

            case >= KEY_ZERO and <= KEY_NINE:
                var numberKey = (int) key - 48;
                var numberText = shift ? CapitalNumbers[numberKey] : $"{numberKey}";
                text = text.Insert(cursorPosition, numberText);
                cursorPosition++;
                break;

            case KEY_LEFT when cursorPosition > 0:
                cursorPosition--;
                break;

            case KEY_RIGHT when cursorPosition < text.Length:
                cursorPosition++;
                break;

            case KEY_BACKSPACE when cursorPosition > 0:
                text = text.Remove(cursorPosition - 1, 1);
                cursorPosition--;
                break;

            case KEY_DELETE when cursorPosition < text.Length:
                text = text.Remove(cursorPosition, 1);
                break;

            case KEY_END:
                cursorPosition = text.Length;
                break;

            case KEY_HOME:
                cursorPosition = 0;
                break;

            case KEY_ENTER:
                FinishTyping();
                break;
            
            default:
                if (shift && CapitalKeyCharacters.ContainsKey(key))
                {
                    text = text.Insert(cursorPosition, CapitalKeyCharacters[key]);
                    cursorPosition++;
                }
                else if (KeyCharacters.ContainsKey(key))
                {
                    text = text.Insert(cursorPosition, KeyCharacters[key]);
                    cursorPosition++;
                }

                break;
        }
    }

    public void FinishTyping()
    {
        Input.MouseOccupier = null;
        textComponent.text = text;
    }

    ~InputBox()
    {
        Input.OnKeyPressed -= (_, key) => KeyEvent(key.key);
        Input.OnKeyRepeat -= (_, key) => KeyEvent(key.key);
    }
}