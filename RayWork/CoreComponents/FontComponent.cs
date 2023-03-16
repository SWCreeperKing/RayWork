using Raylib_CsLo;
using RayWork.ECS;

namespace RayWork.CoreComponents;

public abstract class FontComponent : DebugComponent
{
    public static Font DefaultFont = Raylib.GetFontDefault();

    public Font Font => font ?? DefaultFont;

    public Font? font;
    
    public float fontSize = 24;
    public float spacing = 1.5f;

    public virtual void Debug()
    {
    }
}