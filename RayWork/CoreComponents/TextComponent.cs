using System.Numerics;
using ImGuiNET;
using Raylib_CsLo;

namespace RayWork.CoreComponents;

public class TextComponent : FontComponent
{
    public string text;
    public float rotation = 0;

    public TextComponent(string text)
    {
        this.text = text;
    }

    public void DrawText(Vector2 position, Vector2 origin, Color color)
    {
        Raylib.DrawTextPro(Font, text, position, origin, rotation, fontSize, spacing, color);
    }

    public override void Debug()
    {
        ImGui.InputTextMultiline("Text", ref text, 1024, Vector2.Zero);
        ImGui.InputFloat("Font Size", ref fontSize);
        ImGui.InputFloat("Spacing", ref spacing);

        var rad = rotation / 57.2958f;
        if (ImGui.SliderAngle("Rotation", ref rad)) rotation = rad * 57.2958f;
    }
}