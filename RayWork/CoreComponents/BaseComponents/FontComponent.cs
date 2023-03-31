using System.Numerics;
using Raylib_CsLo;
using RayWork.ECS;

namespace RayWork.CoreComponents;

public abstract class FontComponent : IDebugComponent
{
    public static Font DefaultFont = Raylib.GetFontDefault();

    public Font Font => _Font ?? DefaultFont;
    private Font? _Font;

    public string Text;
    public float FontSize = 24;
    public float Spacing = 1.5f;

    private (string, float, float, Vector2) Cache = ("", 0, 0, Vector2.Zero);

    public Vector2 Size()
    {
        if (Cache.Item1 == Text && Cache.Item2 == FontSize && Cache.Item3 == Spacing) return Cache.Item4;
        var measure = Raylib.MeasureTextEx(Font, Text, FontSize, Spacing);
        Cache = (Text, FontSize, Spacing, measure);

        return Cache.Item4;
    }

    public virtual void Debug()
    {
    }

    public void SetFont(Font font) => _Font = font;
}