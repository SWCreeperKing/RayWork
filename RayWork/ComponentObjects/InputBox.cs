using System.Numerics;
using Raylib_cs;
using RayWork.CoreComponents;
using RayWork.CoreComponents.BaseComponents;
using RayWork.EventArguments;
using RayWork.Objects;
using static Raylib_cs.KeyboardKey;

namespace RayWork.ComponentObjects;

public class InputBox : GameObject
{
    public static readonly string[] CapitalNumbers = [")", "!", "@", "#", "$", "%", "^", "&", "*", "("];

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
        get => PaddingHolder;
        set
        {
            PaddingHolder = value;
            Padding2Holder = value * 2;
        }
    }

    private Vector2 PaddingHolder = new(3);
    private Vector2 Padding2Holder = new(6);

    public string Text = "";
    public string CursorChar = "_";
    public string CursorBlinkChar = " ";
    public int CursorPosition;
    public bool DoCursorBlinking = true;
    public float CursorBlinkCooldownSeconds = .75f;
    public float CursorBlinkDurationSeconds = .25f;

    private bool CursorBlink;
    private float CursorTimer;
    private bool MouseIn;
    private event EventHandler<KeyEvent>? KeyActionEvent;

    public InputBox(TransformComponent transformComponent, SizeComponent sizeComponent)
    {
        AddComponent(TextComponent = new TextComponent(Text));
        AddComponent(PanelComponent =
            new PanelComponent(RectangleComponent = new RectangleComponent(transformComponent, sizeComponent)));
        SetupInputEvent();
    }

    public InputBox(Vector2 position, Vector2 size)
    {
        AddComponent(TextComponent = new TextComponent(Text));
        AddComponent(PanelComponent =
            new PanelComponent(RectangleComponent =
                new RectangleComponent((PositionComponent) position, (StaticSizeComponent) size)));
        SetupInputEvent();
    }

    public override void UpdateLoop()
    {
        MouseIn = PanelComponent.Rectangle.IsMouseIn();

        if (Input.MouseOccupier != this)
        {
            if (MouseIn)
            {
                Input.SetMouseCursor(MouseCursor.MOUSE_CURSOR_IBEAM);
            }

            if (Input.MouseOccupier is not null ||
                !MouseIn ||
                !Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
                return;

            CursorBlink = false;
            CursorTimer = CursorBlinkCooldownSeconds;
            Input.MouseOccupier = this;
        }

        if (MouseIn || !Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
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
        var rectangleSize = RectangleComponent.Size;
        var rectanglePosition = RectangleComponent.Position;
        var textSize = TextComponent.Size();

        var cursorOffset = TextComponent.MeasureText(Text).X - textSize.X;
        var textOffset = textSize.X - cursorOffset - rectangleSize.X;
        var textPosition = rectanglePosition;

        if (textOffset > 0 && Input.MouseOccupier == this)
        {
            var percent = (float) CursorPosition / Text.Length;
            textPosition.X -= textOffset * percent;
        }

        PanelComponent.DrawPanel();
        (rectanglePosition + PaddingHolder).MaskDraw(rectangleSize - Padding2Holder, () =>
            TextComponent.DrawText(textPosition + PaddingHolder));
    }

    private void SetupInputEvent()
    {
        KeyActionEvent = (_, key) => KeyEvent(key.Key);
        Input.OnKeyPressed += KeyActionEvent;
        Input.OnKeyRepeat += KeyActionEvent;
    }

    private void KeyEvent(KeyboardKey key)
    {
        if (Input.MouseOccupier != this) return;
        var shift = Raylib.IsKeyDown(KEY_LEFT_SHIFT) || Raylib.IsKeyDown(KEY_RIGHT_SHIFT);
        var ctrl = Raylib.IsKeyDown(KEY_LEFT_CONTROL) || Raylib.IsKeyDown(KEY_RIGHT_CONTROL);

        switch (key)
        {
            case KEY_C when ctrl:
                Raylib.SetClipboardText(Text);
                break;

            case KEY_V when ctrl:
                var clipboardText = Raylib.GetClipboardText_();
                Text = Text.Insert(CursorPosition, clipboardText);
                CursorPosition += clipboardText.Length;
                break;

            case KEY_X when ctrl:
                Raylib.SetClipboardText(Text);
                CursorPosition = 0;
                Text = "";
                break;

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
                if (!ctrl)
                {
                    Text = Text.Remove(--CursorPosition, 1);
                }
                else
                {
                    var lastSpace = Math.Max(0, Text[..CursorPosition].LastIndexOf(' '));
                    Text = Text.Remove(lastSpace, CursorPosition - lastSpace);
                    CursorPosition = lastSpace;
                }

                break;

            case KEY_DELETE when Text.Length > 0 && ctrl && shift:
                CursorPosition = 0;
                Text = "";
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
                if (shift && CapitalKeyCharacters.TryGetValue(key, out var capitalKeyCharacter))
                {
                    Text = Text.Insert(CursorPosition, capitalKeyCharacter);
                    CursorPosition++;
                }
                else if (KeyCharacters.TryGetValue(key, out var keyCharacter))
                {
                    Text = Text.Insert(CursorPosition, keyCharacter);
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
        Input.OnKeyPressed -= KeyActionEvent;
        Input.OnKeyRepeat -= KeyActionEvent;
    }

    public override MouseCursor OccupiedMouseCursor()
        => MouseIn ? MouseCursor.MOUSE_CURSOR_IBEAM : MouseCursor.MOUSE_CURSOR_DEFAULT;
}