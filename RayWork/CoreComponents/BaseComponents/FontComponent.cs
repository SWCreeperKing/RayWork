using System.Numerics;
using Raylib_cs;
using RayWork.ECS;

namespace RayWork.CoreComponents.BaseComponents;

public abstract class FontComponent : DebugComponent
{
    public static Font DefaultFont = Raylib.GetFontDefault();

    public Font Font => FontHolder ?? DefaultFont;
    private Font? FontHolder;

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

    public void SetFont(Font font) => FontHolder = font;
    public Vector2 MeasureText(string text) => Raylib.MeasureTextEx(Font, text, FontSize, Spacing);
}