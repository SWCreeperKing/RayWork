using System.Numerics;
using ImGuiNET;
using Raylib_CsLo;
using RayWork.Objects;

namespace RayWork.CoreComponents;

public class TextComponent : FontComponent
{
    public float rotation;
    public CompatibleColor color;

    public TextComponent(string text, Color? color = null)
    {
        this.text = text;
        this.color = color ?? Raylib.BLACK;
    }

    public void DrawText(Vector2 position, Vector2 origin)
    {
        Raylib.DrawTextPro(Font, text, position, origin, rotation, fontSize, spacing, color);
    }

    public override void Debug()
    {
        ImGui.InputTextMultiline("Text", ref text, 1024, Vector2.Zero);
        ImGui.InputFloat("Font Size", ref fontSize);
        ImGui.InputFloat("Spacing", ref spacing);
        color.ImGuiColorEdit("Color");

        var rad = rotation / 57.2958f;
        if (ImGui.SliderAngle("Rotation", ref rad)) rotation = rad * 57.2958f;
    }

    public static implicit operator TextComponent(string text) => new(text);
}