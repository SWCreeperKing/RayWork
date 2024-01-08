using System.Numerics;
using ImGuiNET;
using Raylib_cs;
using RayWork.RLImgui;

namespace RayWork.Objects;

public class CompatibleColor
{
    public Color Color
    {
        get => _Color;
        set
        {
            R = value.R;
            G = value.G;
            B = value.B;
            A = value.A;
        }
    }

    public Vector4 ColorV4
    {
        get => _ColorV4;
        set
        {
            R = (byte) (value.X * 255);
            G = (byte) (value.Y * 255);
            B = (byte) (value.Z * 255);
            A = (byte) (value.W * 255);
        }
    }

    public byte R
    {
        get => _Color.R;
        set
        {
            _Color.R = value;
            _ColorV4.X = value / 255f;
        }
    }

    public byte G
    {
        get => _Color.G;
        set
        {
            _Color.G = value;
            _ColorV4.Y = value / 255f;
        }
    }

    public byte B
    {
        get => _Color.B;
        set
        {
            _Color.B = value;
            _ColorV4.Z = value / 255f;
        }
    }

    public byte A
    {
        get => _Color.A;
        set
        {
            _Color.A = value;
            _ColorV4.W = value / 255f;
        }
    }

    private Color _Color;
    private Vector4 _ColorV4;

    public CompatibleColor(byte rgb, byte a = 255) : this(rgb, rgb, rgb, a)
    {
    }

    public CompatibleColor(byte r, byte g, byte b, byte a = 255)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public CompatibleColor(Color color) : this(color.R, color.G, color.B, color.A)
    {
    }

    public CompatibleColor(Vector4 colorV4) : this((byte) (colorV4.X * 255), (byte) (colorV4.Y * 255),
        (byte) (colorV4.Z * 255), (byte) (colorV4.W * 255))
    {
    }

    /// <summary>
    /// not recommended
    /// use <see cref="ImGuiColorEdit"/>
    /// </summary>
    /// <param name="label"></param>
    [Obsolete]
    public void ImGuiColorPicker(string label)
    {
        if (!ImGui.ColorPicker4(label, ref _ColorV4)) return;
        _Color = _ColorV4.ToColor();
    }

    public void ImGuiColorEdit(string label)
    {
        if (!ImGui.ColorEdit4(label, ref _ColorV4)) return;
        _Color = _ColorV4.ToColor();
    }

    public CompatibleColor MakeLighter() => new(_Color.MakeLighter());
    public CompatibleColor MakeDarker() => new(_Color.MakeDarker());
    public static implicit operator Color(CompatibleColor compatibleColor) => compatibleColor._Color;
    public static implicit operator Vector4(CompatibleColor compatibleColor) => compatibleColor._ColorV4;
    public static implicit operator CompatibleColor(Color color) => new(color);
    public static implicit operator CompatibleColor(Vector4 colorV4) => new(colorV4);
}