using System.Numerics;
using ImGuiNET;
using Raylib_CsLo;
using RayWork.ECS;
using RayWork.RLImgui;

namespace RayWork.CoreComponents;

public class ColorComponent : DebugComponent
{
    public Color color;

    private Vector4 _colorV4;

    public ColorComponent(short r = 0, short g = 0, short b = 0, short a = 255) : this(new Color(r, g, b, a))
    {
    }

    public ColorComponent(Color color)
    {
        this.color = color;
        _colorV4 = color.ToV4();
    }

    public void Debug()
    {
        if (ImGui.ColorEdit4("Color", ref _colorV4)) SetColor(_colorV4);
    }

    public void SetColor(Vector4 color)
    {
        this.color = color.ToColor();
    }
}