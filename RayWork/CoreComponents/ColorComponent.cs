using Raylib_CsLo;
using RayWork.ECS;
using RayWork.Objects;

namespace RayWork.CoreComponents;

public class ColorComponent : DebugComponent
{
    public CompatibleColor color;
    public string label = "Color";

    public ColorComponent(Color color)
    {
        this.color = color;
    }
    
    public ColorComponent(short r = 0, short g = 0, short b = 0, short a = 255) : this(new Color(r, g, b, a))
    {
    }

    public void Debug()
    {
        color.ImGuiColorEdit(label);
    }

    public static implicit operator ColorComponent(Color color) => new(color);
}