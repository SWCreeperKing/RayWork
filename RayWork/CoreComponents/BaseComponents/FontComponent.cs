using System.Numerics;
using Raylib_CsLo;
using RayWork.ECS;

namespace RayWork.CoreComponents;

public abstract class FontComponent : DebugComponent
{
    public static Font DefaultFont = Raylib.GetFontDefault();

    public Font Font => font ?? DefaultFont;

    public Font? font;

    public string text;
    public float fontSize = 24;
    public float spacing = 1.5f;

    private (string, float, float, Vector2) cache = ("", 0, 0, Vector2.Zero);

    public Vector2 Size()
    {
        if (cache.Item1 != text || cache.Item2 != fontSize || cache.Item3 != spacing)
        {
            var measure = Raylib.MeasureTextEx(Font, text, fontSize, spacing);
            cache = (text, fontSize, spacing, measure);
        }

        return cache.Item4;
    }

    public virtual void Debug()
    {
    }
}