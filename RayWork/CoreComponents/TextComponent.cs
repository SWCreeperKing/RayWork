using System.Numerics;
using ImGuiNET;
using Raylib_cs;
using RayWork.Objects;
using static Raylib_cs.Color;

namespace RayWork.CoreComponents;

public class TextComponent : FontComponent
{
    public float Rotation;
    public CompatibleColor Color;

    public TextComponent(string text, Color? color = null)
    {
        Text = text;
        Color = color ?? BLACK;
    }

    public void DrawText(Vector2 position, Vector2 origin)
        => Raylib.DrawTextPro(Font, Text, position, origin, Rotation, FontSize, Spacing, Color);

    public override void Debug()
    {
        ImGui.InputTextMultiline("Text", ref Text, 1024, Vector2.Zero);
        ImGui.InputFloat("Font Size", ref FontSize);
        ImGui.InputFloat("Spacing", ref Spacing);
        Color.ImGuiColorEdit("Color");

        var rad = Rotation / 57.2958f;
        if (ImGui.SliderAngle("Rotation", ref rad))
        {
            Rotation = rad * 57.2958f;
        }
    }

    public static implicit operator TextComponent(string text) => new(text);
}