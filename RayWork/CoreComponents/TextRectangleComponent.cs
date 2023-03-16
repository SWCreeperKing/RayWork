using System.Numerics;
using ImGuiNET;
using Raylib_CsLo;

namespace RayWork.CoreComponents;

public class TextRectangleComponent : FontComponent
{
    public string text;
    public bool wordWrap = true;

    public TextRectangleComponent(string text)
    {
        this.text = text;
    }

    public void DrawText(Color color, Rectangle rectangle)
    {
        Font.DrawTextRec(text, rectangle, color, fontSize, spacing, wordWrap);
    }

    public override void Debug()
    {
        ImGui.InputTextMultiline("Text", ref text, 1024, Vector2.Zero);
        ImGui.Checkbox("Word Wrap", ref wordWrap);
        ImGui.InputFloat("Font Size", ref fontSize);
        ImGui.InputFloat("Spacing", ref spacing);
    }
}