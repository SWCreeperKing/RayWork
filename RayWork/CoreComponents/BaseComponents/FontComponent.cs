using System.Numerics;
using Raylib_cs;
using RayWork.ECS;

namespace RayWork.CoreComponents.BaseComponents;

public abstract class FontComponent : DebugComponent
{
    public static Font DefaultFont = Raylib.GetFontDefault();

    public Font Font => _Font ?? DefaultFont;
    private Font? _Font;

    public string Text = "";
    public float FontSize = 24;
    public float Spacing = 1.5f;

    private (string, float, float, Vector2) Cache = ("", 0, 0, Vector2.Zero);

    public Vector2 Size()
    {
        if (Cache.Item1 == Text && Cache.Item2 == FontSize && Cache.Item3 == Spacing) return Cache.Item4;
        var measure = MeasureText(Text);
        Cache = (Text, FontSize, Spacing, measure);

        return Cache.Item4;
    }

    public void SetFont(Font font) => _Font = font;
    public Vector2 MeasureText(string text) => Raylib.MeasureTextEx(Font, text, FontSize, Spacing);

    public static Vector2 MeasureDefText(string text, float fontSize = 24, float spacing = 1.5f)
        => Raylib.MeasureTextEx(DefaultFont, text, fontSize, spacing);
}