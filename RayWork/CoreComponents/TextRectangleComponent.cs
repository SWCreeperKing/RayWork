using System.Numerics;
using ImGuiNET;
using Raylib_CsLo;
using RayWork.Objects;
using Rectangle = RayWork.Objects.Rectangle;
using RayRectangle = Raylib_CsLo.Rectangle;

namespace RayWork.CoreComponents;

public class TextRectangleComponent : FontComponent
{
    public bool WordWrap = true;
    public CompatibleColor Color;

    public TextRectangleComponent(string text, Color? color = null)
    {
        Text = text;
        Color = color ?? Raylib.BLACK;
    }

    public void DrawText(Rectangle rectangle) => Font.DrawTextRec(Text, rectangle, Color, FontSize, Spacing, WordWrap);

    public void DrawText(RayRectangle rectangle)
        => Font.DrawTextRec(Text, rectangle, Color, FontSize, Spacing, WordWrap);

    public override void Debug()
    {
        ImGui.InputTextMultiline("Text", ref Text, 1024, Vector2.Zero);
        ImGui.Checkbox("Word Wrap", ref WordWrap);
        ImGui.InputFloat("Font Size", ref FontSize);
        ImGui.InputFloat("Spacing", ref Spacing);
        Color.ImGuiColorEdit("Color");
    }

    public static implicit operator TextRectangleComponent(string text) => new(text);
}