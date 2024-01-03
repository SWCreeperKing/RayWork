using Raylib_cs;
using RayWork.ECS;
using RayWork.Objects;

namespace RayWork.CoreComponents;

public class ColorComponent : IDebugComponent
{
    public CompatibleColor Color;
    public string Label = "Color";

    public ColorComponent(Color color)
    {
        Color = color;
    }

    public ColorComponent(short r = 0, short g = 0, short b = 0, short a = 255) : this(new Color(r, g, b, a))
    {
    }

    public void Debug() => Color.ImGuiColorEdit(Label);
    public static implicit operator ColorComponent(Color color) => new(color);
}