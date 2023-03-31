using System.Numerics;
using System.Text;
using Raylib_CsLo;
using static Raylib_CsLo.Raylib;

namespace RayWork;

public static class Extensions
{
    public static int maskingLayer;
    private static readonly Dictionary<string, byte[]> StringCache = new();

    public static Vector2 Position(this Rectangle rectangle)
    {
        return new Vector2(rectangle.X, rectangle.Y);
    }

    public static Vector2 Size(this Rectangle rectangle)
    {
        return new Vector2(rectangle.width, rectangle.height);
    }

    public static bool IsVector2In(this Rectangle rectangle, Vector2 vector2)
    {
        return CheckCollisionPointRec(vector2, rectangle);
    }

    public static bool IsMouseIn(this Rectangle rectangle)
    {
        return rectangle.IsVector2In(GetMousePosition());
    }

    public static Rectangle Rect(this Vector2 position, Vector2 size)
    {
        return new Rectangle(position.X, position.Y, size.X, size.Y);
    }

    public static Texture GetTexture(this Image image)
    {
        return LoadTextureFromImage(image);
    }

    /// <summary>
    /// mask a draw action within the bounds of 2 <see cref="Vector2"/>s
    /// </summary>
    /// <param name="pos">top left of mask</param>
    /// <param name="size">size of mask</param>
    /// <param name="draw">draw action to mask</param>
    public static void MaskDraw(this Vector2 pos, Vector2 size, Action draw)
    {
        maskingLayer++;
        BeginScissorMode((int) pos.X, (int) pos.Y, (int) size.X, (int) size.Y);
        draw();
        if (maskingLayer == 1) EndScissorMode();
        maskingLayer--;
    }

    /// <summary>
    /// makes a <see cref="Color"/> slightly lighter
    /// </summary>
    /// <param name="color"><see cref="Color"/> to make lighter</param>
    /// <returns>the lighter version of <paramref name="color"/></returns>
    public static Color MakeLighter(this Color color)
    {
        return new Color((int) Math.Min(color.r * 1.5, 255), (int) Math.Min(color.g * 1.5, 255),
            (int) Math.Min(color.b * 1.5, 255),
            color.a);
    }

    /// <summary>
    /// makes a <see cref="Color"/> slightly darker
    /// </summary>
    /// <param name="color"><see cref="Color"/> to make darker</param>
    /// <returns>the darker version of <paramref name="color"/></returns>
    public static Color MakeDarker(this Color color)
    {
        return new Color((int) (color.r / 1.7), (int) (color.g / 1.7), (int) (color.b / 1.7), color.a);
    }

    /// <summary>
    /// this draws text in a <see cref="Rectangle"/> and wraps it according to said <see cref="Rectangle"/>
    /// </summary>
    /// <param name="font"><see cref="Font"/> to draw <paramref name="text"/> with</param>
    /// <param name="text">text to draw</param>
    /// <param name="rect">the bounds to draw text around</param>
    /// <param name="fontColor"><see cref="Color"/> of the text</param>
    /// <param name="fontSize">sizeEquation of the text</param>
    /// <param name="spacing">spacing of the characters</param>
    /// <param name="wordWrap">to wrap words instead of just characters</param>
    public static void DrawTextRec(this Font font, string text, Rectangle rect, Color fontColor,
        float fontSize = 24, float spacing = 1.5f, bool wordWrap = true)
    {
        DrawTextRec(font, text, rect, fontColor, fontSize, spacing, wordWrap, 0, 0, WHITE, WHITE);
    }

    /// <inheritdoc cref="DrawTextRec(Font,string,Rectangle,Color,float,float,bool,int,int,Color,Color)"/>
    /// <param name="selectStart">unknown</param>
    /// <param name="selectLength">unknown</param>
    /// <param name="selectTint">unknown</param>
    /// <param name="selectBackTint">unknown</param>
    /// <remarks>this method was removed from Raylib 4.0, but the source code was still in an <a href="https://www.raylib.com/examples/text/loader.html?name=text_rectangle_bounds">Example</a> so the code was copied and fixed to work in C#</remarks>
    public static unsafe void DrawTextRec(Font font, string text, Rectangle rect, Color tint, float fontSize,
        float spacing, bool wordWrap, int selectStart, int selectLength, Color selectTint, Color selectBackTint)
    {
        if (!StringCache.ContainsKey(text)) StringCache[text] = Encoding.ASCII.GetBytes(text);
        var bytes = StringCache[text];

        sbyte* sb;
        fixed (byte* b = bytes) sb = (sbyte*) b;
        var length = TextLength(sb); // Total length in bytes of the text, scanned by codepoints in loop

        var textOffsetY = 0f; // Offset between lines (on line break '\n')
        var textOffsetX = 0f; // Offset X to next character to draw

        var scaleFactor = fontSize / font.baseSize;
        var state = !wordWrap;

        var startLine = -1; // Index where to begin drawing (where a line begins)
        var endLine = -1; // Index where to stop drawing (where a line ends)
        var lastk = -1; // Holds last value of the character position

        for (int i = 0, k = 0; i < length; i++, k++)
        {
            // Get next codepoint from byte string and glyph index in font
            var codepointByteCount = 0;
            var codepoint = GetCodepoint(&sb[i], &codepointByteCount);
            var index = GetGlyphIndex(font, codepoint);

            // NOTE: Normally we exit the decoding sequence as soon as a bad byte is found (and return 0x3f)
            // but we need to draw all of the bad bytes using the '?' symbol moving one byte
            if (codepoint == 0x3f) codepointByteCount = 1;
            i += codepointByteCount - 1;

            float glyphWidth = 0;
            if (codepoint != '\n')
            {
                glyphWidth = font.glyphs[index].advanceX == 0
                    ? font.recs[index].width * scaleFactor
                    : font.glyphs[index].advanceX * scaleFactor;

                if (i + 1 < length) glyphWidth += spacing;
            }

            if (!state)
            {
                if (codepoint is ' ' or '\t' or '\n') endLine = i;

                if (textOffsetX + glyphWidth > rect.width)
                {
                    endLine = endLine < 1 ? i : endLine;
                    if (i == endLine) endLine -= codepointByteCount;
                    if (startLine + codepointByteCount == endLine) endLine = i - codepointByteCount;

                    state = !state;
                }
                else if (i + 1 == length)
                {
                    endLine = i;
                    state = !state;
                }
                else if (codepoint == '\n') state = !state;

                if (state)
                {
                    textOffsetX = 0;
                    i = startLine;
                    glyphWidth = 0;

                    // Save character position when we switch states
                    var tmp = lastk;
                    lastk = k - 1;
                    k = tmp;
                }
            }
            else
            {
                if (codepoint == '\n')
                {
                    if (!wordWrap)
                    {
                        textOffsetY += (font.baseSize + font.baseSize / 2) * scaleFactor;
                        textOffsetX = 0;
                    }
                }
                else
                {
                    if (!wordWrap && textOffsetX + glyphWidth > rect.width)
                    {
                        textOffsetY += (font.baseSize + font.baseSize / 2) * scaleFactor;
                        textOffsetX = 0;
                    }

                    // When text overflows rectangle height limit, just stop drawing
                    if (textOffsetY + font.baseSize * scaleFactor > rect.height) break;

                    // Draw selection background
                    var isGlyphSelected = false;
                    if (selectStart >= 0 && k >= selectStart && k < selectStart + selectLength)
                    {
                        DrawRectangleRec(
                            new Rectangle(rect.X + textOffsetX - 1, rect.Y + textOffsetY, glyphWidth,
                                font.baseSize * scaleFactor), selectBackTint);
                        isGlyphSelected = true;
                    }

                    // Draw current character glyph
                    if (codepoint != ' ' && codepoint != '\t')
                    {
                        DrawTextCodepoint(font, codepoint, new Vector2(rect.X + textOffsetX, rect.Y + textOffsetY),
                            fontSize, isGlyphSelected ? selectTint : tint);
                    }
                }

                if (wordWrap && i == endLine)
                {
                    textOffsetY += (font.baseSize + font.baseSize / 2) * scaleFactor;
                    textOffsetX = 0;
                    startLine = endLine;
                    endLine = -1;
                    glyphWidth = 0;
                    selectStart += lastk - k;
                    k = lastk;
                    state = !state;
                }
            }

            textOffsetX += glyphWidth;
        }
    }
}