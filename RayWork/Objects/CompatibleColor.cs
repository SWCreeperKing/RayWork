using System.Numerics;
using ImGuiNET;
using Raylib_cs;
using RayWork.RLImgui;

namespace RayWork.Objects;

public class CompatibleColor
{
    public Color Color
    {
        get => ColorHolder;
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
        get => ColorV4Holder;
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
        get => ColorHolder.R;
        set
        {
            ColorHolder.R = value;
            ColorV4Holder.X = value / 255f;
        }
    }

    public byte G
    {
        get => ColorHolder.G;
        set
        {
            ColorHolder.G = value;
            ColorV4Holder.Y = value / 255f;
        }
    }

    public byte B
    {
        get => ColorHolder.B;
        set
        {
            ColorHolder.B = value;
            ColorV4Holder.Z = value / 255f;
        }
    }

    public byte A
    {
        get => ColorHolder.A;
        set
        {
            ColorHolder.A = value;
            ColorV4Holder.W = value / 255f;
        }
    }

    private Color ColorHolder;
    private Vector4 ColorV4Holder;

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
        if (!ImGui.ColorPicker4(label, ref ColorV4Holder)) return;
        ColorHolder = ColorV4Holder.ToColor();
    }

    public void ImGuiColorEdit(string label)
    {
        if (!ImGui.ColorEdit4(label, ref ColorV4Holder)) return;
        ColorHolder = ColorV4Holder.ToColor();
    }

    public CompatibleColor MakeLighter() => new(ColorHolder.MakeLighter());
    public CompatibleColor MakeDarker() => new(ColorHolder.MakeDarker());
    public static implicit operator Color(CompatibleColor compatibleColor) => compatibleColor.ColorHolder;
    public static implicit operator Vector4(CompatibleColor compatibleColor) => compatibleColor.ColorV4Holder;
    public static implicit operator CompatibleColor(Color color) => new(color);
    public static implicit operator CompatibleColor(Vector4 colorV4) => new(colorV4);
}