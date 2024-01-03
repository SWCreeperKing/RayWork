using System.Numerics;
using Raylib_cs;
using RayWork.ECS;
using RayWork.Objects;
using Image = Raylib_cs.Image;
using RayRectangle = Raylib_cs.Rectangle;
using Texture = Raylib_cs.Texture2D;

namespace RayWork.CoreComponents;

public class ImageComponent : IDebugComponent
{
    public Image Image;
    public Texture Texture;
    public CompatibleColor Tint;

    public ImageComponent(Image image)
    {
        Image = image;
        Texture = image.GetTexture();
    }

    public ImageComponent(string imagePath) : this(Raylib.LoadImage(imagePath))
    {
    }

    public void Draw(Vector2 position, float rotation = 0, float scale = 1)
        => Raylib.DrawTextureEx(Texture, position, rotation, scale, Tint);

    public void Draw(RayRectangle source, RayRectangle destination, Vector2 origin, float rotation = 0)
        => Raylib.DrawTexturePro(Texture, source, destination, origin, rotation, Tint);

    public virtual void Debug() => Tint.ImGuiColorEdit("Tint");
}