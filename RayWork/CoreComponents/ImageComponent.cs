using System.Numerics;
using Raylib_CsLo;
using RayWork.ECS;
using RayWork.Objects;
using Image = Raylib_CsLo.Image;
using RayRectangle = Raylib_CsLo.Rectangle;

namespace RayWork.CoreComponents;

public class ImageComponent : DebugComponent
{
    public Image image;
    public Texture texture;
    public CompatibleColor tint;

    public ImageComponent(Image image)
    {
        this.image = image;
        texture = image.GetTexture();
    }

    public ImageComponent(string imagePath) : this(Raylib.LoadImage(imagePath))
    {
    }

    public void Draw(Vector2 position, float rotation = 0, float scale = 1)
    {
        Raylib.DrawTextureEx(texture, position, rotation, scale, tint);
    }

    public void Draw(RayRectangle source, RayRectangle destination, Vector2 origin, float rotation = 0)
    {
        Raylib.DrawTexturePro(texture, source, destination, origin, rotation, tint);
    }

    public virtual void Debug()
    {
        tint.ImGuiColorEdit("Tint");
    }
}