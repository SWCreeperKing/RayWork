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
            ColorHolder = value;
            ColorV4Holder = ColorHolder.ToV4();
        }
    }

    public Vector4 ColorV4
    {
        get => ColorV4Holder;
        set
        {
            ColorV4Holder = value;
            ColorHolder = ColorV4Holder.ToColor();
        }
    }

    private Color ColorHolder;
    private Vector4 ColorV4Holder;

    public CompatibleColor(int rgb, int a = 255) : this(new Color(rgb, rgb, rgb, a))
    {
    }
    
    public CompatibleColor(int r, int g, int b, int a = 255) : this(new Color(r, g, b, a))
    {
    }
    
    public CompatibleColor(Color color) => Color = color;
    public CompatibleColor(Vector4 colorV4) => ColorV4 = colorV4;

    /// <summary>
    /// not recommended
    /// use <see cref="ImGuiColorEdit"/>
    /// </summary>
    /// <param name="label"></param>
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