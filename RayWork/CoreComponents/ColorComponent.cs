using System.Numerics;
using ImGuiNET;
using Raylib_CsLo;
using RayWork.ECS;
using RayWork.RLImgui;

namespace RayWork.CoreComponents;

public class ColorComponent : DebugComponent
{
    public Color Color
    {
        get => _color;
        set
        {
            _color = value;
            _colorV4 = _color.ToV4();
        }
    }

    private Vector4 _colorV4;
    private Color _color;

    public ColorComponent(Color color)
    {
        Color = color;
    }
    
    public ColorComponent(short r = 0, short g = 0, short b = 0, short a = 255) : this(new Color(r, g, b, a))
    {
    }

    public void Debug()
    {
        if (ImGui.ColorEdit4("Color", ref _colorV4)) _color = _colorV4.ToColor();
    }
}