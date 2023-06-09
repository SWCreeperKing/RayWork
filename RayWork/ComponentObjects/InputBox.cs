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

    public RectangleComponent RectangleComponent;
    public PanelComponent PanelComponent;
    public TextComponent TextComponent;

    public Vector2 Padding
    {
        get => _Padding;
        set
        {
            _Padding = value;
            _Padding2 = value * 2;
        }
    }

    private Vector2 _Padding = new(3);
    private Vector2 _Padding2 = new(6);

    public string Text = "";
    public string CursorChar = "_";
    public string CursorBlinkChar = " ";
    public int CursorPosition;
    public bool DoCursorBlinking = true;
    public float CursorBlinkCooldownSeconds = .75f;
    public float CursorBlinkDurationSeconds = .25f;

    private bool CursorBlink;
    private float CursorTimer;

    public InputBox(TransformComponent transformComponent, SizeComponent sizeComponent)
    {
        AddComponent(TextComponent = new(Text));
        AddComponent(PanelComponent =
            new(RectangleComponent = new(transformComponent, sizeComponent)));
        SetupInputEvent();
    }

    public InputBox(Vector2 position, Vector2 size)
    {
        AddComponent(TextComponent = new(Text));
        AddComponent(PanelComponent =
            new(RectangleComponent =
                new((PositionComponent) position, (StaticSizeComponent) size)));
        SetupInputEvent();
    }

    public override void UpdateLoop()
    {
        if (Input.MouseOccupier != this)
        {
            if (Input.MouseOccupier is not null ||
                !PanelComponent.RectangleComponent.Rectangle.IsMouseIn() ||
                !Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            {
                return;
            }

            CursorBlink = false;
            CursorTimer = CursorBlinkCooldownSeconds;
            Input.MouseOccupier = this;
        }

        if (PanelComponent.RectangleComponent.Rectangle.IsMouseIn() ||
            !Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
        {
            if (DoCursorBlinking)
            {
                CursorTimer -= RayApplication.DeltaTime;
                if (CursorTimer <= 0)
                {
                    CursorBlink = !CursorBlink;
                    CursorTimer = CursorBlink ? CursorBlinkDurationSeconds : CursorBlinkCooldownSeconds;
                }

                TextComponent.Text = Text.Insert(CursorPosition, CursorBlink ? CursorBlinkChar : CursorChar);
            }
            else
            {
                TextComponent.Text = Text.Insert(CursorPosition, CursorChar);
            }

            return;
        }

        FinishTyping();
    }

    public override void RenderLoop()
    {
        PanelComponent.DrawPanel();
        RectangleComponent.Rectangle.DrawMask(RectangleComponent.Size - _Padding2, () =>
            TextComponent.DrawText(RectangleComponent.Position + _Padding, Vector2.Zero));
    }

    public override MouseCursor OccupiedMouseCursor()
    {
        return MouseCursor.MOUSE_CURSOR_IBEAM;
    }

    private void SetupInputEvent()
    {
        Input.OnKeyPressed += (_, key) => KeyEvent(key.Key);
        Input.OnKeyRepeat += (_, key) => KeyEvent(key.Key);
    }

    private void KeyEvent(KeyboardKey key)
    {
        if (Input.MouseOccupier != this) return;
        var shift = Raylib.IsKeyDown(KEY_LEFT_SHIFT) || Raylib.IsKeyDown(KEY_RIGHT_SHIFT);

        switch (key)
        {
            case >= KEY_A and <= KEY_Z:
                var character = (int) key;
                if (!shift)
                {
                    character += 32;
                }

                Text = Text.Insert(CursorPosition, $"{(char) character}");
                CursorPosition++;
                break;

            case >= KEY_ZERO and <= KEY_NINE:
                var numberKey = (int) key - 48;
                var numberText = shift ? CapitalNumbers[numberKey] : $"{numberKey}";
                Text = Text.Insert(CursorPosition, numberText);
                CursorPosition++;
                break;

            case KEY_LEFT when CursorPosition > 0:
                CursorPosition--;
                break;

            case KEY_RIGHT when CursorPosition < Text.Length:
                CursorPosition++;
                break;

            case KEY_BACKSPACE when CursorPosition > 0:
                Text = Text.Remove(CursorPosition - 1, 1);
                CursorPosition--;
                break;

            case KEY_DELETE when CursorPosition < Text.Length:
                Text = Text.Remove(CursorPosition, 1);
                break;

            case KEY_END:
                CursorPosition = Text.Length;
                break;

            case KEY_HOME:
                CursorPosition = 0;
                break;

            case KEY_ENTER:
                FinishTyping();
                break;

            default:
                if (shift && CapitalKeyCharacters.ContainsKey(key))
                {
                    Text = Text.Insert(CursorPosition, CapitalKeyCharacters[key]);
                    CursorPosition++;
                }
                else if (KeyCharacters.ContainsKey(key))
                {
                    Text = Text.Insert(CursorPosition, KeyCharacters[key]);
                    CursorPosition++;
                }

                break;
        }
    }

    public void FinishTyping()
    {
        Input.MouseOccupier = null;
        TextComponent.Text = Text;
    }

    ~InputBox()
    {
        Input.OnKeyPressed -= (_, key) => KeyEvent(key.Key);
        Input.OnKeyRepeat -= (_, key) => KeyEvent(key.Key);
    }
}