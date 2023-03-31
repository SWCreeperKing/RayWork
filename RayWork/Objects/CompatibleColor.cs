using System.Numerics;
using ImGuiNET;
using Raylib_CsLo;
using RayWork.RLImgui;

namespace RayWork.Objects;

public class CompatibleColor
{
    public Color Color
    {
        get => _Color;
        set
        {
            _Color = value;
            _ColorV4 = _Color.ToV4();
        }
    }

    public Vector4 ColorV4
    {
        get => _ColorV4;
        set
        {
            _ColorV4 = value;
            _Color = _ColorV4.ToColor();
        }
    }

    private Color _Color;
    private Vector4 _ColorV4;

    public CompatibleColor(Color color) => Color = color;
    public CompatibleColor(Vector4 colorV4) => ColorV4 = colorV4;

    public void ImGuiColorPicker(string label)
    {
        if (ImGui.ColorPicker4(label, ref _ColorV4))
        {
            _Color = _ColorV4.ToColor();
        }
    }

    public void ImGuiColorEdit(string label)
    {
        if (ImGui.ColorEdit4(label, ref _ColorV4))
        {
            _Color = _ColorV4.ToColor();
        }
    }

    public static implicit operator Color(CompatibleColor compatibleColor) => compatibleColor._Color;
    public static implicit operator Vector4(CompatibleColor compatibleColor) => compatibleColor._ColorV4;
    public static implicit operator CompatibleColor(Color color) => new(color);
    public static implicit operator CompatibleColor(Vector4 colorV4) => new(colorV4);
}