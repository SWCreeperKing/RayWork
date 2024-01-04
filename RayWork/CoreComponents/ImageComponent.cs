using System.Numerics;
using Raylib_cs;
using RayWork.ECS;
using RayWork.Objects;
using Image = Raylib_cs.Image;
using RayRectangle = Raylib_cs.Rectangle;
using Texture = Raylib_cs.Texture2D;

namespace RayWork.CoreComponents;

public class ImageComponent(Image image) : DebugComponent
{
    public Image Image = image;
    public Texture Texture = image.GetTexture();
    public CompatibleColor Tint = Color.WHITE;

    public ImageComponent(string imagePath) : this(Raylib.LoadImage(imagePath))
    {
    }

    public void Draw(Vector2 position, float rotation = 0, float scale = 1)
        => Raylib.DrawTextureEx(Texture, position, rotation, scale, Tint);

    public void Draw(RayRectangle source, RayRectangle destination, Vector2 origin, float rotation = 0)
        => Raylib.DrawTexturePro(Texture, source, destination, origin, rotation, Tint);

    public override void Debug() => Tint.ImGuiColorEdit("Tint");
}