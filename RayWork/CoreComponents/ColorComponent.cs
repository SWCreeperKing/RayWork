using Raylib_cs;
using RayWork.ECS;
using RayWork.Objects;

namespace RayWork.CoreComponents;

public class ColorComponent(Color color, string label = "Color") : IDebugComponent
{
    public CompatibleColor Color = color;
    public string Label = label;

    public ColorComponent(short r = 0, short g = 0, short b = 0, short a = 255) : this(new Color(r, g, b, a))
    {
    }

    public void Debug() => Color.ImGuiColorEdit(Label);
    public static implicit operator ColorComponent(Color color) => new(color);
}