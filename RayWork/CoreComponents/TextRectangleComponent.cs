using System.Numerics;
using ImGuiNET;
using Raylib_CsLo;
using RayWork.Objects;

namespace RayWork.CoreComponents;

public class TextRectangleComponent : FontComponent
{
    public bool wordWrap = true;
    public CompatibleColor color;
    
    public TextRectangleComponent(string text, Color? color = null)
    {
        this.text = text;
        this.color = color ?? Raylib.BLACK;
    }

    public void DrawText(Rectangle rectangle)
    {
        Font.DrawTextRec(text, rectangle, color, fontSize, spacing, wordWrap);
    }

    public override void Debug()
    {
        ImGui.InputTextMultiline("Text", ref text, 1024, Vector2.Zero);
        ImGui.Checkbox("Word Wrap", ref wordWrap);
        ImGui.InputFloat("Font Size", ref fontSize);
        ImGui.InputFloat("Spacing", ref spacing);
        color.ImGuiColorEdit("Color");
    }

    public static implicit operator TextRectangleComponent(string text) => new(text);
}