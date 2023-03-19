using System.Numerics;
using ImGuiNET;
using Raylib_CsLo;
using RayWork.RLImgui;

namespace RayWork.Objects;

public class CompatibleColor
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

    public Vector4 ColorV4
    {
        get => _colorV4;
        set
        {
            _colorV4 = value;
            _color = _colorV4.ToColor();
        }
    }

    private Color _color;
    private Vector4 _colorV4;

    public CompatibleColor(Color color)
    {
        Color = color;
    }

    public CompatibleColor(Vector4 colorV4)
    {
        ColorV4 = colorV4;
    }

    public void ImGuiColorPicker(string label)
    {
        if (ImGui.ColorPicker4(label, ref _colorV4)) _color = _colorV4.ToColor();
    }

    public void ImGuiColorEdit(string label)
    {
        if (ImGui.ColorEdit4(label, ref _colorV4)) _color = _colorV4.ToColor();
    }

    public static implicit operator Color(CompatibleColor compatibleColor) => compatibleColor._color;
    public static implicit operator Vector4(CompatibleColor compatibleColor) => compatibleColor._colorV4;
    public static implicit operator CompatibleColor(Color color) => new(color);
    public static implicit operator CompatibleColor(Vector4 colorV4) => new(colorV4);
}